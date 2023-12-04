﻿using CommunityToolkit.Mvvm.Input;
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

namespace SmartShopMobileApp.ViewModels
{
    public partial class PaymentViewModel:ObservableObject
    {
        public PaymentViewModel() 
        {
           
        }

        private double totalAmount;
        public double TotalAmount {
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
                Currency = "ron",
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
                Currency = "RON",
                ReceiptEmail = "patyanelis@yahoo.com",
                Customer = cust.Id,
                Source = source.Id
            };

            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeoption);
            if (charge.Status == "succeeded")
            {
                Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Payment Confirmation", "The transaction was successful! You have recieved an email to confirm your payment.", "OK");
                SendPaymentConfirmationEmail("patyanelis@yahoo.com", TotalAmount); // sa modific cu adresa userului conectat dupa autentificare!!
                App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Oops!", "Something went wrong!", "OK");
                return;
            }
        }
        public async void SendPaymentConfirmationEmail(string recipientEmail, double totalAmount)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("smartshopapp.testing@gmail.com"));
            email.To.Add(MailboxAddress.Parse("patyanelis@yahoo.com"));  // sa modific cu adresa userului conectat dupa autentificare!!
            email.Subject = $"Payment Confirmation";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<p>Dear customer,</p>" + // sa inlocuiesc customer cu numele utilizatorului
                     $"<p>We are pleased to inform you that your payment of {TotalAmount} RON has been successfully processed. Thank you for your payment! </p>" + 
                     $"<p> With respect,  </p>" +
                     $"<p> SmartShop Mobile App <3 </p> "
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("smartshopapp.testing@gmail.com", "xfky mnvc wevt azih");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        private void UpdatePayButtonText()
        {
            PayButtonText = $"Pay {TotalAmount} lei";
        }

        [RelayCommand]
        private void Pay()
        {
            PayViaStripe();
        }
    }
}
