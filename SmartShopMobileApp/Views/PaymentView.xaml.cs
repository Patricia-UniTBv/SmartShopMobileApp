using Stripe;
using Stripe.Infrastructure;
using System.ComponentModel;

namespace SmartShopMobileApp.Views;

[DesignTimeVisible(false)]
public partial class PaymentView : ContentPage
{
	public PaymentView()
	{
		InitializeComponent();
    }

    public void PayViaStripe()
	{
		StripeConfiguration.ApiKey = "sk_test_51OHNMNDQz7fQ3QseH9lroYrCZXFfNVJAqJeHgWgeOYjczfYfH2M7lCJiTsnjb5gSysFLcdVT5wdVYjYt3gD2SVCs00acLz6SUz";

        string cardno = cardNo.Text;
		string expMonth = expireMonth.Text;
		string expYear = expireYear.Text;
		string cardCvv = cvv.Text;

		
		Stripe.TokenCardOptions stripeOption = new Stripe.TokenCardOptions();
		stripeOption.Number = cardno;
        stripeOption.ExpYear = expYear;
        stripeOption.ExpMonth = expMonth;
        stripeOption.Cvc = cardCvv;

        TokenCreateOptions stripeCard = new TokenCreateOptions();
        stripeCard.Card = stripeOption;

        TokenService service = new TokenService();
        Token newToken = service.Create(stripeCard); // https://stackoverflow.com/questions/76583126/sending-credit-card-numbers-directly-to-the-stripe-api-is-generally-unsafe-we-s

        var option = new SourceCreateOptions
        {
            Type = SourceType.Card,
            Currency = "ron",
            Token = newToken.Id
        };

        var sourceService = new SourceService();
        Source source = sourceService.Create(option);

        CustomerCreateOptions customer = new CustomerCreateOptions
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
            Amount = 2,
            Currency = "RON",
            ReceiptEmail = "patyanelis@yahoo.com",
            Customer = cust.Id,
            Source = source.Id
        };

        var chargeService = new ChargeService();
        Charge charge = chargeService.Create(chargeoption);
        if (charge.Status == "succeeded")
        {
            // success
        }
        else
        {
            // failed
        }


    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        PayViaStripe();
    }
}