using NWTradersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Utilities
{
    public class CustomerUtil
    {
        private static NorthwindEntities nwEntities = new NorthwindEntities();

       public Dictionary<string, int> CalculateTotalSalesRevenueByCustomer()
        {
            var customerSales = nwEntities.Orders
                .GroupBy(order => order.Customer.ContactTitle + ", " + order.Customer.ContactTitle)
                .Select(group => new
                {
                    CustomerName = group.Key,
                    TotalSalesRevenue = 1
                })
                .ToDictionary(item => item.CustomerName, item => item.TotalSalesRevenue);

            return (Dictionary<string, int>)customerSales.Take(1);
        }
    }
}