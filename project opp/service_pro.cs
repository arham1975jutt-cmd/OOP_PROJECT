
using System;
using System.Collections.Generic;
using System.Linq;
using AlHadiTraders.Models;
using AlHadiTraders.Repository;

namespace AlHadiTraders.Services
{
    public class TradersService
    {
        private TradersRepo repo;

        public TradersService()
        {
            repo = new TradersRepo();
        }

        public List<Product> GetProducts()
        {
            return repo.GetProducts();
        }

        public List<Customer> GetCustomers()
        {
            return repo.GetCustomers();
        }

        public List<Sale> GetSales()
        {
            return repo.GetSales();
        }

        public void AddSale(string name, List<SaleItem> items)
        {
            Sale sale = new Sale();

            sale.CustomerName = name;
            sale.Items = items;

            repo.AddSale(sale);
        }

        public float TotalSale()
        {
            float total = 0;

            List<Sale> sales = repo.GetSales();

            foreach (Sale sale in sales)
            {
                total += sale.Total;
            }

            return total;
        }

        public bool DeleteCustomer(string name)
        {
            List<Sale> sales = repo.GetSales();

            int count = sales.RemoveAll(
                s => string.Equals(
                    s.CustomerName,
                    name,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (count > 0)
            {
                repo.SaveSales(sales);
                return true;
            }

            return false;
        }

        public void SortDescending()
        {
            List<Sale> sales = repo.GetSales();

            sales = sales.OrderByDescending(
                s => s.Total
            ).ToList();

            repo.SaveSales(sales);
        }

        public bool UpdatePrice(string name, float price)
        {
            if (price <= 0)
                return false;

            List<Product> products = repo.GetProducts();

            Product product = products.FirstOrDefault(
                p => string.Equals(
                    p.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (product == null)
                return false;

            product.Price = price;

            repo.SaveProducts(products);

            return true;
        }

        public bool ReturnProduct(
            string customerName,
            string productName,
            float quantity)
        {
            if (quantity <= 0)
                return false;

            List<Sale> sales = repo.GetSales();

            Sale sale = sales
                .OrderByDescending(s => s.Id)
                .FirstOrDefault(s =>
                    string.Equals(
                        s.CustomerName,
                        customerName,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (sale == null || sale.Items == null)
                return false;

            SaleItem item = sale.Items.FirstOrDefault(
                i => string.Equals(
                    i.ProductName,
                    productName,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (item == null || quantity > item.Quantity)
                return false;

            item.Quantity -= quantity;

            if (item.Quantity == 0)
                sale.Items.Remove(item);

            repo.SaveSales(sales);

            return true;
        }
    }
}
