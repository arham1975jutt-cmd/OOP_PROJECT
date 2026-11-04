using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using AlHadiTraders.Models;



namespace AlHadiTraders.Repository
{
    public class TradersRepo
    {
        private string productFile = "products.txt";
        private string salesFile = "sales.txt";

        // GET ALL PRODUCTS

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            if (!File.Exists(productFile))
            {
                products.Add(new Product("seeds", 600));
                products.Add(new Product("soda", 170));
                products.Add(new Product("glucose", 400));
                products.Add(new Product("yeast", 550));

                SaveProducts(products);
                return products;
            }

            string[] lines = File.ReadAllLines(productFile);

            foreach (string line in lines)
            {
                string[] data = line.Split('|');

                if (data.Length == 2)
                {
                    products.Add(new Product(
                        data[0],
                        float.Parse(data[1])
                    ));
                }
            }

            return products;
        }

        // SAVE PRODUCTS

        public void SaveProducts(List<Product> products)
        {
            List<string> lines = new List<string>();

            foreach (Product p in products)
            {
                lines.Add(p.Name + "|" + p.Price);
            }

            File.WriteAllLines(productFile, lines);
        }

        // GET ALL SALES

        public List<Sale> GetSales()
        {
            List<Sale> sales = new List<Sale>();

            if (!File.Exists(salesFile))
                return sales;

            string[] lines = File.ReadAllLines(salesFile);

            foreach (string line in lines)
            {
                string[] data = line.Split('|');

                if (data.Length != 5)
                    continue;

                int id = int.Parse(data[0]);

                Sale sale = sales.FirstOrDefault(s => s.Id == id);

                if (sale == null)
                {
                    sale = new Sale();

                    sale.Id = id;
                    sale.CustomerName = data[1];

                    sales.Add(sale);
                }

                SaleItem item = new SaleItem(
                    data[2],
                    float.Parse(data[3]),
                    float.Parse(data[4])
                );

                sale.Items.Add(item);
            }

            return sales;
        }

        // SAVE SALES

        public void SaveSales(List<Sale> sales)
        {
            List<string> lines = new List<string>();

            foreach (Sale sale in sales)
            {
                foreach (SaleItem item in sale.Items)
                {
                    string line =
                        sale.Id + "|" +
                        sale.CustomerName + "|" +
                        item.ProductName + "|" +
                        item.Quantity + "|" +
                        item.Price;

                    lines.Add(line);
                }
            }

            File.WriteAllLines(salesFile, lines);
        }

        // ADD SALE

        public void AddSale(Sale sale)
        {
            List<Sale> sales = GetSales();

            if (sales.Count == 0)
                sale.Id = 1;
            else
                sale.Id = sales.Max(s => s.Id) + 1;

            sales.Add(sale);

            SaveSales(sales);
        }

        // GET CUSTOMERS

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            List<Sale> sales = GetSales();

            foreach (Sale sale in sales)
            {
                bool exists = customers.Any(
                    c => c.Name.ToLower() ==
                    sale.CustomerName.ToLower()
                );

                if (!exists)
                {
                    customers.Add(new Customer(sale.CustomerName));
                }
            }

            return customers;
        }
    }
}