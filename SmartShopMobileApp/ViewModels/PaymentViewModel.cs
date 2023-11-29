using SmartShopMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public class PaymentViewModel
    {
        private ObservableCollection<Cards> cards;
        private ObservableCollection<Invoice> invoice;

        public ObservableCollection<Cards> Cards
        {
            get { return cards; }
            set { this.cards = value; }
        }

        public ObservableCollection<Invoice> Invoice
        {
            get { return invoice; }
            set { this.invoice = value; }
        }


        public PaymentViewModel()
        {
            cards = new ObservableCollection<Cards>();
            cards.Add(new Cards { Picture = "card" });
            cards.Add(new Cards { Picture = "card2" });

            invoice = new ObservableCollection<Invoice>();
            invoice.Add(new Invoice { Description = "Subtotal", Price = "219$" });
            invoice.Add(new Invoice { Description = "Postage", Price = "3.9$" });
            invoice.Add(new Invoice { Description = "Tax", Price = "12.45$" });
        }
    }
}
