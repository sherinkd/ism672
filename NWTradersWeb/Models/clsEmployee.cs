using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Models
{
    public partial class Employee
    {
        public List<BestSellingProduct> BestSellingProducts(int limit)
        {
            return Orders
                .SelectMany(s => s.Order_Details)
                .Select(od => od.Product)
                .GroupBy(s => s.ProductName, s => s.ProductID, (key, g) => new BestSellingProduct { ProductName = key, NumberOfSales = g.ToList().Count() })
                .OrderByDescending(s => s.NumberOfSales)
                .Take(limit)
                .ToList();
        }
    }

    public class BestSellingProduct
    {
        public string ProductName { get; set; }
        public int NumberOfSales { get; set; }
    }
}