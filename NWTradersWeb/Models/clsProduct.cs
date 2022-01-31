using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NWTradersWeb.Models
{
    public class ProductMetadata
    {
        public int ProductID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CategoryID { get; set; }


        /// <summary>
        /// Company Name- 
        /// Required, Minimum - 5 characters
        /// </summary>
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "The Product Name is required")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Product Name must have atleast 3 and upto 40 characters")]
        public string ProductName { get; set; }


        /// <summary>
        /// Company Name- 
        /// Required, Minimum - 5 characters
        /// </summary>
        [Display(Name = "Quantity Per Unit")]
        // Not [Required(ErrorMessage = "The Product Name is required")]
        [StringLength(20, ErrorMessage = "Quantity per unit cannot have more than 20 characters")]
        public string QuantityPerUnit { get; set; }


        [Required(ErrorMessage = "The unit price cannot be blank")]
        [Range(0, 10000, ErrorMessage = "Enter a price between 0 and 10000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Display(Name = "Price Per Unit")]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "Units in Stock")]
        [Range(0, 10000, ErrorMessage = "Enter the number of units, between 0 and 10,000 ")]
        public Nullable<short> UnitsInStock { get; set; }

        [Display(Name = "Units on Order")]
        [Range(0, 10000, ErrorMessage = "Enter the units on Order, between 0 and 10,000 ")]
        public Nullable<short> UnitsOnOrder { get; set; }


        [Display(Name = "ReOrder level")]
        [Range(0, 10000, ErrorMessage = "Enter the # of units, between 0 and 10,000, when an automatic re-order is triggered ")]
        public Nullable<short> ReorderLevel { get; set; }

        [Display(Name = "Product Discontinued?")]
        [DefaultValue(false)]
        public bool Discontinued { get; set; }


    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }
}