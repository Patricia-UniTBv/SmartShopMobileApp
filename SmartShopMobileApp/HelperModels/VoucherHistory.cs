using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.HelperModels
{
    public partial class VoucherHistory: INotifyPropertyChanged
    {
        public string CartCreationDate { get; set; }
        public DateTime CreationDate { get; set; }

        public string TotalAmount { get; set; }

        public bool? IsTransacted { get; set; }

        public string ValueModification { get; set; }

        private Color _textColor;

        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
