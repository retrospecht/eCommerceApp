using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Specht2025_Samples.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private ProductServiceProxy _prodSvc = ProductServiceProxy.Current;
        private List<Item> items;
        public double TaxRate { get; set; } = 0.07; 
        public double? Subtotal => CartItems.Sum(i => i.Price * i.Quantity);
        public double? Tax => Math.Round((double)(Subtotal * TaxRate), 2);
        public double? Total => Math.Round((double)(Subtotal + Tax), 2);

        public List<Item> CartItems
        {
            get
            {
                return items;
            }
        }
        public static ShoppingCartService Current
        {
            get
            {
                if(instance == null)
                {
                    instance = new ShoppingCartService();
                }

                return instance;
            }
        }

        private static ShoppingCartService? instance;
        private ShoppingCartService() 
        {
            items = new List<Item>();
        }

        public Item? AddOrUpdate(Item item)
        {
            var existingInvItem = _prodSvc.GetById(item.Id);
            if(existingInvItem == null || existingInvItem.Quantity == 0)
            {
                return null;
            }

            if (existingInvItem != null)
            {
                existingInvItem.Quantity--;
            }
            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id);
            if(existingItem == null)
            {
                var newItem = new Item(item);
                newItem.Quantity = 1;
                CartItems.Add(new Item(newItem));
            }
            else
            {
                existingItem.Quantity++;
            }

            return existingInvItem;

        }

        public Item? ReturnItem(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }

            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null)
            {
                itemToReturn.Quantity--;
                var inventoryItem = _prodSvc.Products.FirstOrDefault(p => p.Id == itemToReturn.Id);
                if(inventoryItem == null)
                {
                    _prodSvc.AddOrUpdate(new Item(itemToReturn));
                }
                else
                {
                    inventoryItem.Quantity++;
                }
            }

      

            return itemToReturn;
        }

        public string GetReceipt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("===== Receipt =====");
            sb.AppendLine();

            foreach (var item in CartItems)
            {
                double itemTotal = (double)(item.Price * item.Quantity);
                sb.AppendLine($"{item.Product.Name} x{item.Quantity} @ ${item.Price:F2} = ${itemTotal:F2}");
            }

            sb.AppendLine();
            sb.AppendLine($"Subtotal: ${Subtotal:F2}");
            sb.AppendLine($"Tax ({(TaxRate * 100):F2}%): ${Tax:F2}");
            sb.AppendLine($"Total: ${Total:F2}");
            sb.AppendLine("===================");

            return sb.ToString();
        }

        public void UpdateTaxRate(double newTaxRate)
        {
                TaxRate = newTaxRate; 
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }

}
