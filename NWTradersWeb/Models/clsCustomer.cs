using System;
using System.Linq;

namespace NWTradersWeb.Models
{


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

    }

}