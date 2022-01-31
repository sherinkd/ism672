using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NWTradersWeb.Models
{

    /// Annotations MetaData and Decorations for EmpCode.
    public class CustomerMetadata
    {

        /// <summary>
        /// User Name
        /// Required, Minimum - 5 characters
        /// </summary>
        [Display(Name = "Customer ID")]
        [Required(ErrorMessage = "The Customer ID is required")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Customer ID must have atleast 3 characters")]
        public string CustomerID;

        /// <summary>
        /// Company Name- 
        /// Required, Minimum - 5 characters
        /// </summary>
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "The Company Name is required")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Company Name must have atleast 5 characters")]
        public string CompanyName;

        [Display(Name = "Contact Name")]
        [Required(ErrorMessage = "The Contact Name is required")]
        [StringLength(30)]
        public string ContactName;

        [Display(Name = "Contact Title")]
        [StringLength(30)]
        public string ContactTitle;

        [Display(Name = "Address")]
        [StringLength(60)]
        public string Address;

        [Display(Name = "City")]
        [StringLength(15)]
        public string City;

        [Display(Name = "Region")]
        [StringLength(15)]
        public string Region;

        // Not -Required fields:
        [Display(Name = "Postal Code")]
        [StringLength(10)]
        public string PostalCode;

        [Display(Name = "Country")]
        [StringLength(15)]
        public string Country;

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"1?\W*([2-9][0-8][0-9])\W*([2-9][0-9]{2})\W*([0-9]{4})(\se?x?t?(\d*))?",
            ErrorMessage = "Please enter a valid Phone number")]
        [StringLength(24)]
        public string Phone;

        [Display(Name = "Fax")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"1?\W*([2-9][0-8][0-9])\W*([2-9][0-9]{2})\W*([0-9]{4})(\se?x?t?(\d*))?",
            ErrorMessage = "Please enter a valid Phone number")]
        [StringLength(24)]
        public string Fax;
    }

    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }

}