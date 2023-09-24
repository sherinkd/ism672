using NWTradersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Utilities
{
    public class ProductUtil
    {
        private static NorthwindEntities nwEntities = new NorthwindEntities();

        public static List<string> AllCategories() => nwEntities
               .Categories
               .Select(c => c.CategoryName)               
               .Distinct()
               .OrderBy(c => c)
               .ToList();


        public static List<string> AllSuppliers() => nwEntities
       .Suppliers
       .Select(s => s.CompanyName)
       .Distinct()
       .OrderBy(s => s)
       .ToList();
    }
}