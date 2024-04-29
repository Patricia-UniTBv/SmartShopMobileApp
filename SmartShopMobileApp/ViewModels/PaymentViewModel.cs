using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Stripe;
using Stripe.Checkout;
using Stripe.Infrastructure;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using SmartShopMobileApp.Helpers;
using Newtonsoft.Json;
using SmartShopMobileApp.Views;
using System.Collections.ObjectModel;
using SmartShopMobileApp.Services.Interfaces;
using SmartShopMobileApp.Services;

namespace SmartShopMobileApp.ViewModels
{
    public partial class PaymentViewModel:ObservableObject
    {
        public PaymentViewModel() 
        {
            _manageData = new ManageData();
            _authService = new AuthService();

            ActiveUser = new AuthResponseDTO();

            CurrencyValue = PreferredCurrency.Value;
        }

        private IManageData _manageData;

        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        private IAuthService _authService;
        public IAuthService AuthService
        {
            get { return _authService; }
            set { _authService = value; }
        }

        private decimal totalAmount;
        public decimal TotalAmount {
            get { return totalAmount; }
            set
            {
                if (totalAmount != value)
                {
                    totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                    UpdatePayButtonText();
                }
            }
        }
        public int ShoppingCartId { get; set; }

        public decimal VoucherDiscount { get;set; }

        [ObservableProperty]
        public ObservableCollection<string> _paymentMethods;

        [ObservableProperty]
        public bool _isSaveCardChecked;

        [ObservableProperty]
        public string _cardNo;

        [ObservableProperty]
        public string _expirationYear;

        [ObservableProperty]
        public string _expirationMonth;

        [ObservableProperty]
        public string _cvv;

        [ObservableProperty]
        public string _payButtonText;

        [ObservableProperty]
        private string _currencyValue;

        [ObservableProperty]
        private AuthResponseDTO _activeUser;

        public void PayViaStripe()
        {
            StripeConfiguration.ApiKey = "sk_test_51OHNMNDQz7fQ3QseH9lroYrCZXFfNVJAqJeHgWgeOYjczfYfH2M7lCJiTsnjb5gSysFLcdVT5wdVYjYt3gD2SVCs00acLz6SUz";

            string cardno = CardNo;
            string expMonth = ExpirationMonth;
            string expYear = ExpirationYear;
            string cardCvv = Cvv;

            TokenCreateOptions tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = cardno,
                    ExpMonth = expMonth,
                    ExpYear = expYear,
                    Cvc = cardCvv
                }
            };

            TokenService tokenService = new TokenService();
            Token stripeToken = tokenService.Create(tokenOptions);

            var option = new SourceCreateOptions
            {
                Type = SourceType.Card,
                Currency = CurrencyValue,
                Token = stripeToken.Id
            };

            var sourceService = new SourceService();
            Source source = sourceService.Create(option);


            CustomerCreateOptions customer = new CustomerCreateOptions // se modifica dupa autentificare!
            {
                Name = "Patricia Anghelache",
                Email = "patricia.anghelache@student.unitbv.ro",
                Description = "Pay",
                Address = new AddressOptions { City = "Brasov", Country = "Romania", Line1 = "Sample Address", Line2 = "Sample Address 2", PostalCode = "700030", State = "" }
            };

            var customerService = new CustomerService();
            var cust = customerService.Create(customer);

            var chargeoption = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(TotalAmount * 100), 
                Currency = CurrencyValue,
                ReceiptEmail = ActiveUser.Email,
                Customer = cust.Id,
                Source = source.Id
            };

            var setupIntentCreateOptions = new SetupIntentCreateOptions
            {
                Customer = cust.Id,
            };

            var setupIntentService = new SetupIntentService();
            var setupIntent = setupIntentService.Create(setupIntentCreateOptions);

          
            var customerId = cust.Id; 
            var paymentMethods = GetCustomerPaymentMethods(customerId);

            foreach (var paymentMethod in paymentMethods)
            {
                string last4Digits ="***********" + paymentMethod.Card?.Last4;

                PaymentMethods.Add(last4Digits);
            }

            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeoption);
            if (charge.Status == "succeeded")
            {
                TransactionDTO transaction = new TransactionDTO();
                transaction.ShoppingCartID = ShoppingCartId;
                transaction.TransactionDate = DateTime.Now;
                transaction.TotalAmount = TotalAmount;
                transaction.VoucherDiscount = VoucherDiscount;
                var json = JsonConvert.SerializeObject(transaction);

                _manageData.SetStrategy(new CreateData());
                var result = _manageData.GetDataAndDeserializeIt<TransactionDTO>("Transaction/AddTransaction", json);

                _manageData.SetStrategy(new UpdateData());
                _manageData.GetDataAndDeserializeIt<object>($"Voucher/UpdateVoucherForSpecificUser/{ActiveUser.UserId}/{CurrentSupermarket.Supermarket.SupermarketID}/{TotalAmount}", "");
                _manageData.GetDataAndDeserializeIt<object>($"ShoppingCart/UpdateShoppingCartWhenTransacted?id={ShoppingCartId}", "");

                Thread.Sleep(1000);

                Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Payment Confirmation", "The transaction was successful! You have recieved an email to confirm your payment.", "OK");
                SendPaymentConfirmationEmail(ActiveUser.Email, TotalAmount); 
                App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new GeneratedQRCodeToExitShopView()));
            }
            else
            {
                Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Oops!", "Something went wrong!", "OK");
                return;
            }
        }
        public async void SendPaymentConfirmationEmail(string recipientEmail, decimal totalAmount)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("smartshopapp.testing@gmail.com"));
            email.To.Add(MailboxAddress.Parse(ActiveUser.Email)); 
            email.Subject = $"Payment Confirmation";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<p>Dear {ActiveUser.FirstName},</p>" + 
                     $"<p>We are pleased to inform you that your payment of {TotalAmount} {CurrencyValue} has been successfully processed. Thank you for your payment! </p>" + 
                     $"<p> With respect,  </p>" +
                     $"<p> SmartShop Mobile App </p> "
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("smartshopapp.testing@gmail.com", "xfky mnvc wevt azih");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public List<PaymentMethod> GetCustomerPaymentMethods(string customerId)
        {
            StripeConfiguration.ApiKey = "sk_test_51OHNMNDQz7fQ3QseH9lroYrCZXFfNVJAqJeHgWgeOYjczfYfH2M7lCJiTsnjb5gSysFLcdVT5wdVYjYt3gD2SVCs00acLz6SUz";

            var paymentMethodService = new PaymentMethodService();

            var paymentMethods = paymentMethodService.List(new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card", 
            });

            return paymentMethods.Data;
        }

        private void UpdatePayButtonText()
        {
            PayButtonText = $"Pay {TotalAmount}" + " " + CurrencyValue;
        }

        [RelayCommand]
        private void Pay()
        {
            PayViaStripe();
        }
    }
}
