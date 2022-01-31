using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{
    public class Order_DetailMetadata
    {
        [Required(ErrorMessage = "The unit price cannot be blank")]
        [Range(0, 10000, ErrorMessage = "Enter a price between 0 and 10000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        [Display(Name = "Price Per Unit")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]
        public decimal UnitPrice { get; set; }


        [Required(ErrorMessage = "The quantity cannot be be blank")]
        [Range(0, 10000, ErrorMessage = "Enter a quantity between 0 and 10000.00")] // Edit this for returns
        [Display(Name = "Quantity Ordered")]
        public short Quantity { get; set; }


        [Required(ErrorMessage = "The unit price cannot be blank")]
        [Range(0, 10000, ErrorMessage = "Enter a price between 0 and 10000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        [Display(Name = "Discount applied (Per Unit Price)")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]
        [DefaultValue(0.00)]
        public float Discount { get; set; }


        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0: $#,##0.00}")]
        public decimal Total { get; set; }

    }

    [MetadataType(typeof(Order_DetailMetadata))]
    public partial class Order_Detail
    {

        public string OrderDetailInformation()
        {
            string orderDetailInformation = "";
            orderDetailInformation +=
                this.Product.ProductName + "\t" + "\t" +
               this.Quantity + "\t" + "\t" +
                this.UnitPrice.ToString("C") + "\t" + "\t" +
                this.Discount.ToString("C") + "\t" + "\t" +
                Total.ToString("C") + "\t";
                
            return orderDetailInformation;
        }

        public decimal Total
        {
            get
            {                return ((UnitPrice - (decimal) Discount) * Quantity);}
        }
    }

}
