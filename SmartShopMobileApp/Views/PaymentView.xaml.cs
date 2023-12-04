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
using Microsoft.Maui.Graphics; 

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

        payButton.BackgroundColor = Color.FromArgb("BFC6BF");
        payButton.IsEnabled = false;

        cardNo.TextChanged += EntryTextChanged;
        expireYear.TextChanged += EntryTextChanged;
        expireMonth.TextChanged += EntryTextChanged;
        cvv.TextChanged += EntryTextChanged;
    }
    private void EntryTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdatePayButtonState();
    }

    private void UpdatePayButtonState()
    {
        bool allEntriesCompleted = !string.IsNullOrEmpty(cardNo.Text)
            && !string.IsNullOrEmpty(expireYear.Text)
            && !string.IsNullOrEmpty(expireMonth.Text)
            && !string.IsNullOrEmpty(cvv.Text);

        payButton.IsEnabled = allEntriesCompleted;
        payButton.BackgroundColor = allEntriesCompleted ? Color.FromArgb("24B024") : Color.FromArgb("BFC6BF");
    }
}