using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AlHadiTraders.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }

        public List<SaleItem> Items { get; set; }

        public float Total
        {
            get
            {
                float total = 0;

                foreach (SaleItem item in Items)
                {
                    total += item.Total;
                }

                return total;
            }
        }

        public Sale()
        {
            Items = new List<SaleItem>();
            CustomerName = "";
        }
    }
}