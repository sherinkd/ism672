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

        [Required(ErrorMessage = "The Required Date is required")]
        [Display(Name = "Required Date")]        
        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RequiredDate { get; set; }

        
        [Display(Name = "Shipped Date")]
        [DisplayFormat(DataFormatString = "{0: MMM dd, yyyy}", ApplyFormatInEditMode = true)]        
        public Nullable<System.DateTime> ShippedDate { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        [Display(Name = "Order Total")]
        public decimal orderTotal { get; set; }

        [Required(ErrorMessage = "The Ship Via is required")]
        [Display(Name = "Ship Via")]
        public Nullable<int> ShipVia { get; set; }


        [Display(Name = "Freight")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        public Nullable<decimal> Freight { get; set; }

        // Shipping Variables - These should default to the Customer Address
        // You should allow thew user to change them.
        [Required(ErrorMessage = "The Ship Name is required")]
        [Display(Name = "Ship Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Ship Name must have atleast 3 and upto 40 characters")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "The Ship Address is required")]
        [Display(Name = "Ship Address")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Ship Address must have atleast 3 and upto 60 characters")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "The Ship City is required")]
        [Display(Name = "Ship City")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Ship City must have atleast 3 and upto 15 characters")]
        public string ShipCity { get; set; }

        [Required(ErrorMessage = "The Ship Region is required")]
        [Display(Name = "Ship Region")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Ship Region must have atleast 3 and upto 15 characters")]
        public string ShipRegion { get; set; }

        [Required(ErrorMessage = "The Ship PostalCode is required")]
        [Display(Name = "Ship PostalCode")]
        [DataType(DataType.PostalCode)]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Ship PostalCode must have atleast 5 and upto 10 characters")]
        public string ShipPostalCode { get; set; }

        [Required(ErrorMessage = "The Ship Country is required")]
        [Display(Name = "Ship Country")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Ship Country must have atleast 2 and upto 15 characters")]
        public string ShipCountry { get; set; }

    }
}