using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{
    public partial class Order_Detail
    {

        public string OrderDetailInformation()
        {
            string orderDetailInformation = "";
            orderDetailInformation +=
                this.Product.ProductName + "\t" + "\t" +
               this.Quantity + "\t" + "\t" +
                this.UnitPrice.ToString("c") + "\t" + "\t" +
                this.Discount.ToString("c") + "\t" + "\t" +
                Total.ToString("c") + "\t";
                
            return orderDetailInformation;
        }


        /// <summary>
        ///  The Total for any OrderDetail is the Total Amount for the line Item
        ///  A line item is like a single line on a receipt or invoice - 
        ///  it represents the sales from a single product and 
        ///  is calculated by subtracting the Discount from the price of the Item and 
        ///  multiplying the result by the number of units (Quantity).
        /// </summary>
        public decimal Total
        {
            get
            {                return ((UnitPrice - (decimal) Discount) * Quantity);}
        }
    }

}
