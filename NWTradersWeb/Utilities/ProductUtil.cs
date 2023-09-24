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
               .Products
               .Select(c => c.Category.CategoryName)               
               .Distinct()
               .OrderBy(c => c)
               .ToList();
    }
}