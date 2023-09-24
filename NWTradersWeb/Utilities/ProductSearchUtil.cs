using NWTradersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Utilities
{
    public class ProductSearchUtil
    {
        private List<Product> products = new List<Product>();
        public ProductSearchUtil(
            List<Product> products
            )
        {
            this.products = products;
        }

        public ProductSearchUtil ByProductName(
            string productName
         ) {
            if (string.IsNullOrWhiteSpace(productName)) { return this; }

            products = products
                    .Where(p => p.ProductName.ToUpper().Contains(productName.ToUpper()))
                    .ToList();
            return this;
        }

        public List<Product> GetProducts() {  return products; }
    }
}