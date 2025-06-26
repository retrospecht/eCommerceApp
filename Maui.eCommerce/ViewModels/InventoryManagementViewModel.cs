using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Specht2025_Samples.Models;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }

        private ProductServiceProxy _svc = ProductServiceProxy.Current;

        public ObservableCollection<string> SortOptions { get; } = new ObservableCollection<string>
        {
            "Name",
            "Price (Low to High)",
            "Price (High to Low)"
        };

        private string _selectedSortOption = "Name";
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    NotifyPropertyChanged();
                    RefreshProductList(); // Re-sort the list when the sort option changes
                }
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

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public ObservableCollection<Item?> Products
        {
            get
            {
                IEnumerable<Item?> filteredList = _svc.Products;

                if (!string.IsNullOrWhiteSpace(Query))
                {
                    filteredList = filteredList.Where(p =>
                        p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                }

                switch (SelectedSortOption)
                {
                    case "Name":
                        filteredList = filteredList.OrderBy(item => item?.Product?.Name);
                        break;
                    case "Price (Low to High)":
                        filteredList = filteredList.OrderBy(item => item?.Price);
                        break;
                    case "Price (High to Low)":
                        filteredList = filteredList.OrderByDescending(item => item?.Price);
                        break;
                    default:
                        filteredList = filteredList.OrderBy(item => item?.Product?.Name); // Default
                        break;
                }

                return new ObservableCollection<Item?>(filteredList);
            }
        }

        public Item? Delete()
        {
            var item = _svc.Delete(SelectedProduct?.Id ?? 0);
            RefreshProductList();
            return item;
        }
    }
}