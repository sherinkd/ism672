using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{

    public partial class Order
    {
        public decimal orderTotal
        {
            get
            { return this.Order_Details.Sum(od => od.Total); }
        }

        public decimal TotalSales
        {
            get
            { return this.Order_Details.Sum(od => od.Total); }
        }


        /// <summary>
        /// This function is no longer needed or useed because we are not displaying strings in Web Applications.
        /// </summary>
        /// <returns></returns>
        public string OrderInformation()
        {

            string orderInformation = "";

            orderInformation += "Order ID :" + "\t" + this.OrderID + "\n";
            orderInformation += "Order Date :" + "\t" + this.OrderDate.Value.ToShortDateString() + "\n";
            orderInformation += "--------------------------------------------------------------------------------------- \n";

            foreach (Order_Detail theOrderDetail in this.Order_Details)
            {
                orderInformation += theOrderDetail.OrderDetailInformation() + "\n";
            }
            return orderInformation;


        }
    }
}
