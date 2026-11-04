using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlHadiTraders.Models
{
    public class SaleItem
    {
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }

        public float Total
        {
            get { return Quantity * Price; }
        }

        public SaleItem(string name, float quantity, float price)
        {
            ProductName = name;
            Quantity = quantity;
            Price = price;
        }
    }
}