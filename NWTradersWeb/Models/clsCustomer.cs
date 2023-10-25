using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NWTradersWeb.Models
{

    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer
    {
        /// <summary>
        /// Returns the date when the last order was placed by a particular customer. 
        /// If no orders are placed, the function returns a <see langword="null"/> DateTime Object
        /// </summary>
        /// <returns></returns>
        public DateTime? LastOrderPlacedOn()
        {
            DateTime? lastOrderPlacedOn = null;

            if (Orders.Count() > 0)
            lastOrderPlacedOn=    Orders.
                       Where(o => o.OrderDate.HasValue).
                       OrderByDescending(o => o.OrderDate).
                       Select(o => o.OrderDate.Value).
                       FirstOrDefault();
            return lastOrderPlacedOn;
        }

        public List<Order> RecentOrders(int limit) { 
            return Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(limit)
                .ToList();
        }

        public List<Product> RecentlyPurchasedProducts(int limit)
        {
            return Orders
                .OrderByDescending(o => o.OrderDate)
                .SelectMany(o => o.Order_Details)
                .Select(od => od.Product)
                .Distinct()
                .Take(limit)
                .ToList();
        }

    }

}