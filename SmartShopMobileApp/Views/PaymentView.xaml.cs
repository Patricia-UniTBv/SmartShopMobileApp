using Stripe;
using Stripe.Checkout;
using Stripe.Infrastructure;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmartShopMobileApp.ViewModels;

namespace SmartShopMobileApp.Views;

[DesignTimeVisible(false)]
public partial class PaymentView : ContentPage
{
	public PaymentView(double amount)
	{
		InitializeComponent();
        PaymentViewModel viewModel = new PaymentViewModel();
        viewModel.TotalAmount = amount;
        BindingContext = viewModel;
    }
}