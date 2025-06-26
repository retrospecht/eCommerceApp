using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class CheckoutViewModel 
    {

        private ShoppingCartService _svc = ShoppingCartService.Current;

        public CheckoutViewModel()
        {
            ReceiptText = ShoppingCartService.Current.GetReceipt();
            ShoppingCartService.Current.ClearCart();
        }

        public string ReceiptText { get; set; }
    }
}
