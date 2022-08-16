using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{

    /// <summary>
    /// A customer can place many orders. 
    /// Every order contains (or is associated with the multiple Order_Details (line Items) on that order.
    /// 
    /// To Note: 
    /// Why is it a "Public" Class?
    /// Why is this a partial class?
    /// 
    /// </summary>
    public partial class Order
    {

        /// <summary>
        /// The total for the order is calculated as the sum of order_Detail Total 
        /// for every order_detail on (associated with) the Order.
        /// </summary>
        public decimal orderTotal
        {
            get
            { return this.Order_Details.Sum(od => od.Total); }
        }

    }
}
