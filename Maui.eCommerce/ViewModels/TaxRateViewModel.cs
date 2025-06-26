using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class TaxRateViewModel : INotifyPropertyChanged
    {
        private ShoppingCartService _cartSvc = ShoppingCartService.Current;

        private double _userInputTaxRate;

        public double TaxRate => _cartSvc.TaxRate; // Read current tax rate from ShoppingCartService

        public double UserInputTaxRate
        {
            get => _userInputTaxRate;
            set
            {
                if (_userInputTaxRate != value)
                {
                    _userInputTaxRate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateTaxRate()
        {
            _cartSvc.TaxRate = UserInputTaxRate;
            NotifyPropertyChanged(nameof(TaxRate)); 
        }
    }
}