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

        public ProductSearchUtil ByName(
            string name
         ) {
            if (string.IsNullOrWhiteSpace(name)) { return this; }

            products = products
                    .Where(p => p.ProductName.ToUpper().Contains(name.ToUpper()))
                    .ToList();
            return this;
        }

        public ProductSearchUtil ByCategory(
                string category
        )
        {
            if (string.IsNullOrWhiteSpace(category)) { return this; }

            products = products
                    .Where(p => p.Category.CategoryName.Equals(category))
                    .ToList();
            return this;
        }

        public ProductSearchUtil BySupplier(
                string supplier
        )
        {
            if (string.IsNullOrWhiteSpace(supplier)) { return this; }

            products = products
                    .Where(p => p.Supplier.CompanyName.Equals(supplier))
                    .ToList();
            return this;
        }

        public List<Product> GetProducts() {  return products; }
    }
}