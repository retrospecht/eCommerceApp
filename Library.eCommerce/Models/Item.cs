using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Library.eCommerce.DTO;
using Library.eCommerce.Services;
using Specht2025_Samples.Models;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }

        public ProductDTO Product { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public double ItemTotal => (double)(Price * Quantity);

        public ICommand? AddCommand { get; set; }

        public ICommand? RemoveCommand { get; set;}

        public int TextBoxQuantity { get; set; } = 1;

        public override string ToString()
        {
            return $"{Product} Quantity:{Quantity} Price: {Price}";
        }

        public string Display
        {
            get
            {
                return $"{Product?.Display ?? string.Empty} {Quantity}";
            }
        }

        public Item()
        {
            Product = new ProductDTO();
            Quantity = 0;
            Price = 0.0;

            AddCommand = new Command(DoAdd);
            RemoveCommand = new Command(DoRemove);
        }

        private void DoAdd()
        {
            for (int i = 0; i < TextBoxQuantity; i++)
            {
                ShoppingCartService.Current.AddOrUpdate(this);
            }
        }
        private void DoRemove()
        {
            ShoppingCartService.Current.ReturnItem(this);
        }

        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
            Price = i.Price;

            AddCommand = new Command(DoAdd);
            RemoveCommand = new Command(DoRemove);
        }
    }
}
