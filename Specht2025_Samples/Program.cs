//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Library.eCommerce.Services;
using Specht2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Principal;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? role = "Z";
            do
            {
                Console.WriteLine("Welcome to Amazon!");
                Console.WriteLine("E for Employee, C for Customer, Q to Quit");
                role = Console.ReadLine() ?? "Z";
                if (role[0] == 'E' || role[0] == 'e')
                {
                    Console.WriteLine("Employee View");
                    Employee();
                }
                else if (role[0] == 'C' || role[0] == 'c')
                {
                    Console.WriteLine("Customer View");
                    Customer();
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again");
                }
            } while (role[0] != 'Q' && role[0] != 'q');
        }

        static void Employee()
        {
            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("Q. Quit");

            List<Product?> list = ProductServiceProxy.Current.Products;

            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        Console.Write("Enter a product name: ");
                        string? name = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter a quantity: ");
                        int? quantity = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter a price: ");
                        double? price = double.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = name,
                            Quantity = quantity,
                            Price = price
                        });
                        break;
                    case 'R':
                    case 'r':
                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        Console.WriteLine("Which product would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);
                        if (selectedProd != null)
                        {
                            Console.Write("Enter a new product name: ");
                            name = Console.ReadLine() ?? string.Empty;
                            Console.Write("Enter a new quantity: ");
                            quantity = int.Parse(Console.ReadLine() ?? "0");
                            Console.Write("Enter a new price: ");
                            price = double.Parse(Console.ReadLine() ?? "0");
                            selectedProd.Name = name;
                            selectedProd.Quantity = quantity;
                            selectedProd.Price = price;
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;
                    case 'D':
                    case 'd':
                        Console.WriteLine("Which product would you like to update?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');
            return;
        }
        static void Customer()
        {
            Console.WriteLine("C. Add to shopping cart");
            Console.WriteLine("R. Read all items in shopping cart");
            Console.WriteLine("U. Edit number of products");
            Console.WriteLine("D. Delete an item from your cart");
            Console.WriteLine("V. View inventory");
            Console.WriteLine("O. Check out");
            Console.WriteLine("Q. Quit");

            List<Product?> inventory = ProductServiceProxy.Current.Products;
            List<Product?> shoppingCart = ShoppingCartService.Current.Products;

            char choice;
            do
            {
                string? input = Console.ReadLine() ?? "Z";
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        Console.WriteLine("Which item would you like to add to the cart?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == selection);
                        ShoppingCartService.Current.AddToCart(selectedProd);
                        break;
                    case 'R':
                    case 'r':
                        shoppingCart.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        Console.WriteLine("Which product would you like to edit?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProd = ShoppingCartService.Current.Products.FirstOrDefault(p => p.Id == selection);
                        Console.WriteLine("Would you like to Increase or Decrease the quantity?");
                        string editChoice = Console.ReadLine() ?? "Z";
                        if (editChoice[0] == 'I' || editChoice[0] == 'i')
                            ShoppingCartService.Current.AddToCart(selectedProd);
                        else if (editChoice[0] == 'D' || editChoice[0] == 'd')
                            ShoppingCartService.Current.DecreaseCart(selectedProd);
                        else
                            Console.WriteLine("Invalid Choice");
                        break;
                    case 'D':
                    case 'd':
                        Console.WriteLine("Which product would you like to remove from your cart?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ShoppingCartService.Current.Delete(selection);
                        break;
                    case 'V':
                    case 'v':
                        Console.WriteLine("Inventory: ");
                        inventory.ForEach(Console.WriteLine);
                        break;
                    case 'O':
                    case 'o':
                        Console.WriteLine("Receipt: ");
                        for (int i = 0; i < shoppingCart.Count(); i++)
                        {
                            Console.WriteLine(shoppingCart[i]);
                        }
                        Console.Write("Sub-Total: $");
                        Console.WriteLine(ShoppingCartService.Current.SubTotal());
                        Console.Write("Grand Total: $");
                        Console.WriteLine(ShoppingCartService.Current.GTotal());
                        ShoppingCartService.Current.ClearCart();
                        choice = 'Q';
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');
            return;
        }
    }
}
