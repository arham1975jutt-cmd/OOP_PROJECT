
using System;
using System.Collections.Generic;
using System.Linq;
using AlHadiTraders.Models;
using AlHadiTraders.Services;

namespace AlHadiTraders.UI
{
    public class MainUI
    {
        private TradersService service;

        public MainUI()
        {
            service = new TradersService();
        }

        // MAIN MENU
        public void Menu()
        {
            int choice;

            do
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("       AL HADI TRADERS");
                Console.WriteLine("================================");

                Console.WriteLine("1. Admin Panel");
                Console.WriteLine("2. Salesman Panel");
                Console.WriteLine("3. Exit");

                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    choice = 0;

                if (choice == 1)
                {
                    AdminPanel();
                }
                else if (choice == 2)
                {
                    SalesmanPanel();
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Thank You!");
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                    Console.ReadKey();
                }

            } while (choice != 3);
        }

        // ADMIN PANEL
        public void AdminPanel()
        {
            Console.Clear();

            Console.WriteLine("========== ADMIN LOGIN ==========");

            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (username != "admin" || password != "123")
            {
                Console.WriteLine("Wrong Username or Password");
                Console.ReadKey();
                return;
            }

            int choice;

            do
            {
                Console.Clear();

                Console.WriteLine("========== ADMIN PANEL ==========");

                Console.WriteLine("1. View Customers");
                Console.WriteLine("2. Delete Customer");
                Console.WriteLine("3. Total Sale");
                Console.WriteLine("4. Sort Descending Sale");
                Console.WriteLine("5. Return Product");
                Console.WriteLine("6. Update Price");
                Console.WriteLine("7. Exit");

                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    choice = 0;

                if (choice == 1)
                {
                    ViewCustomers();
                }
                else if (choice == 2)
                {
                    DeleteCustomer();
                }
                else if (choice == 3)
                {
                    ShowTotalSale();
                }
                else if (choice == 4)
                {
                    service.SortDescending();
                    Console.WriteLine("Sorted Successfully");
                }
                else if (choice == 5)
                {
                    ReturnProduct();
                }
                else if (choice == 6)
                {
                    UpdatePrice();
                }
                else if (choice == 7)
                {
                    Console.WriteLine("Admin Logout");
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                }

                if (choice != 7)
                    Console.ReadKey();

            } while (choice != 7);
        }

        // VIEW CUSTOMERS
        public void ViewCustomers()
        {
            Console.Clear();

            Console.WriteLine("========== CUSTOMER LIST ==========");

            List<Customer> customers = service.GetCustomers();

            foreach (Customer c in customers)
            {
                Console.WriteLine("Customer: " + c.Name);
            }

            Console.WriteLine("\n========== SALES DETAILS ==========");

            List<Sale> sales = service.GetSales();

            foreach (Sale sale in sales)
            {
                Console.WriteLine("\nCustomer: " + sale.CustomerName);

                foreach (SaleItem item in sale.Items)
                {
                    Console.WriteLine(
                        item.ProductName +
                        " Quantity: " + item.Quantity +
                        " Price: " + item.Price +
                        " Total: " + item.Total
                    );
                }

                Console.WriteLine("Total Bill: " + sale.Total);
            }
        }

        // DELETE CUSTOMER
        public void DeleteCustomer()
        {
            Console.Write("Enter Customer Name: ");

            string name = Console.ReadLine();

            bool result = service.DeleteCustomer(name);

            if (result)
                Console.WriteLine("Customer Deleted Successfully");
            else
                Console.WriteLine("Customer Not Found");
        }

        // TOTAL SALE
        public void ShowTotalSale()
        {
            float total = service.TotalSale();

            Console.WriteLine("Total Sale: " + total);
        }

        // RETURN PRODUCT
        private void ReturnProduct()
        {
            Console.Write("Enter Customer Name: ");
            string customer = Console.ReadLine();

            Console.Write("Enter Product Name: ");
            string product = Console.ReadLine();

            Console.Write("Enter Quantity: ");

            if (!float.TryParse(Console.ReadLine(), out float quantity)
                || quantity <= 0)
            {
                Console.WriteLine("Invalid Quantity");
                return;
            }

            bool result = service.ReturnProduct(
                customer, product, quantity
            );

            if (result)
                Console.WriteLine("Product Returned Successfully");
            else
                Console.WriteLine("Invalid Customer, Product or Quantity");
        }

        // UPDATE PRICE
        public void UpdatePrice()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter New Price: ");

            if (!float.TryParse(Console.ReadLine(), out float price)
                || price <= 0)
            {
                Console.WriteLine("Invalid Price");
                return;
            }

            bool result = service.UpdatePrice(name, price);

            if (result)
                Console.WriteLine("Price Updated Successfully");
            else
                Console.WriteLine("Product Not Found or Invalid Price");
        }

        // SALESMAN PANEL
        public void SalesmanPanel()
        {
            Console.Clear();

            Console.WriteLine("========== SALESMAN PANEL ==========");

            Console.Write("Enter Customer Name: ");
            string name = Console.ReadLine();

            Console.Write("How Many Products: ");

            if (!int.TryParse(Console.ReadLine(), out int count)
                || count <= 0)
            {
                Console.WriteLine("Invalid Product Count");
                Console.ReadKey();
                return;
            }

            List<SaleItem> items = new List<SaleItem>();

            for (int i = 0; i < count; i++)
            {
                Console.Clear();

                Console.WriteLine("========== AVAILABLE PRODUCTS ==========");

                List<Product> products = service.GetProducts();

                foreach (Product p in products)
                {
                    Console.WriteLine(
                        p.Name + " Price: " + p.Price
                    );
                }

                Console.Write("\nEnter Product Name: ");
                string productName = Console.ReadLine();

                Product selected = products.Find(
                    p => p.Name.Equals(
                        productName,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

                if (selected == null)
                {
                    Console.WriteLine("Product Not Found");
                    i--;
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Enter Quantity: ");

                if (!float.TryParse(Console.ReadLine(), out float qty)
                    || qty <= 0)
                {
                    Console.WriteLine("Invalid Quantity");
                    i--;
                    Console.ReadKey();
                    continue;
                }

                SaleItem item = new SaleItem(
                    selected.Name,
                    qty,
                    selected.Price
                );

                items.Add(item);

                Console.WriteLine("Product Total: " + item.Total);

                Console.ReadKey();
            }

            service.AddSale(name, items);

            Console.Clear();

            Console.WriteLine("========== CUSTOMER BILL ==========");

            Console.WriteLine("Customer Name: " + name);

            float total = 0;

            foreach (SaleItem item in items)
            {
                Console.WriteLine(
                    item.ProductName +
                    " | Quantity: " + item.Quantity +
                    " | Price: " + item.Price +
                    " | Total: " + item.Total
                );

                total += item.Total;
            }

            Console.WriteLine("--------------------------------");

            Console.WriteLine("Total Bill: " + total);

            Console.WriteLine("--------------------------------");

            Console.WriteLine("Sale Saved Successfully");

            Console.ReadKey();
        }
    }
}
