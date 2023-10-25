using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Models
{
    public class OrderMetadata
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Placed On: ")]
        public Nullable<System.DateTime> OrderDate { get; set; }

        //Add annotions for RequiredDate
        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RequiredDate { get; set; }

        //Add annotions for ShippedDate
        public Nullable<System.DateTime> ShippedDate { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        [Display(Name = "Order Total")]
        public decimal orderTotal { get; set; }

        public Nullable<int> ShipVia { get; set; }


        //Add annotation for Freight
        public Nullable<decimal> Freight { get; set; }

        // Shipping Variables - These should default to the Customer Address
        // You should allow thew user to change them.
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

    }
}