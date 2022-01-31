using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{


    public class OrderMetadata
    {

        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}")]
        [Display(Name = "Order Placed On: ")]
        public Nullable<System.DateTime> OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}")]
        [Display(Name = "Order Required On: ")]
        public Nullable<System.DateTime> RequiredDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}")]
        [Display(Name = "Order Shipped On: ")]
        public Nullable<System.DateTime> ShippedDate { get; set; }



        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        [Display(Name = "Order Total")]
        public decimal orderTotal { get; set; }

        public Nullable<int> ShipVia { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        public Nullable<decimal> Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

    }

    [MetadataType(typeof(OrderMetadata))]
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
