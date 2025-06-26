using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private ShoppingCartService _cartSvc = ShoppingCartService.Current;

        public Item? SelectedItem { get; set; }
        public Item? SelectedCartItem { get; set; }

        public ObservableCollection<string> SortOptions { get; } = new ObservableCollection<string>
        {
            "Inventory: Name",
            "Inventory: Price (Low to High)",
            "Inventory: Price (High to Low)",
            "Cart: Name",
            "Cart: Price (Low to High)",
            "Cart: Price (High to Low)"
        };

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart)); // Refresh both lists
                }
            }
        }

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                IEnumerable<Item?> sortedInventory = _invSvc.Products.Where(i => i.Quantity > 0);

                switch (SelectedSortOption)
                {
                    case "Inventory: Name":
                        sortedInventory = sortedInventory.OrderBy(item => item?.Product?.Name);
                        break;
                    case "Inventory: Price (Low to High)":
                        sortedInventory = sortedInventory.OrderBy(item => item?.Price);
                        break;
                    case "Inventory: Price (High to Low)":
                        sortedInventory = sortedInventory.OrderByDescending(item => item?.Price);
                        break;
                }

                return new ObservableCollection<Item?>(sortedInventory);
            }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                IEnumerable<Item?> sortedCart = _cartSvc.CartItems.Where(i => i.Quantity > 0);

                switch (SelectedSortOption)
                {
                    case "Cart: Name":
                        sortedCart = sortedCart.OrderBy(item => item?.Product?.Name);
                        break;
                    case "Cart: Price (Low to High)":
                        sortedCart = sortedCart.OrderBy(item => item?.Price);
                        break;
                    case "Cart: Price (High to Low)":
                        sortedCart = sortedCart.OrderByDescending(item => item?.Price);
                        break;
                }

                return new ObservableCollection<Item?>(sortedCart);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem);
                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public void RefreshUX()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        public void ReturnItem()
        {
            if (SelectedCartItem != null)
            {
                var shouldRefresh = SelectedCartItem.Quantity >= 1;
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem);
                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }
    }
}