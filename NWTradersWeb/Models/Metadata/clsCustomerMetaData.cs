using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Models
{
    public class CustomerMetaData
    {
        [Display(Name = "Customer ID")]
        [Required(ErrorMessage = "The Customer ID is required")]        
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Customer ID must have atleast 3 and upto 5 characters")]
        public string CustomerID;

        [Required(ErrorMessage = "The Customer Company Name is required")]
        [Display(Name = "Customer Company Name ")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Company Name must have atleast 3 and upto 40 characters")]
        public string CompanyName;

        [Required(ErrorMessage = "The Contact Name is required")]
        [Display(Name = "Contact Name ")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Contact Name must have atleast 3 and upto 30 characters")]
        public string ContactName;

        [Required(ErrorMessage = "The Contact Title is required")]
        [Display(Name = "Contact Title ")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Contact Title must have atleast 3 and upto 30 characters")]
        public string ContactTitle;

        [Required(ErrorMessage = "The Address is required")]
        [Display(Name = "Address ")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Address must have atleast 3 and upto 60 characters")]
        public string Address;

        [Required(ErrorMessage = "The City is required")]
        [Display(Name = "City ")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "City must have atleast 3 and upto 15 characters")]
        public string City;
        
        [StringLength(13, ErrorMessage = "Region cannot have more than 13 characters")]
        public string Region;

        [Required(ErrorMessage = "The Postal Code is required")]
        [Display(Name = "Postal Code")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Postal Code must have atleast 4 and upto 10 characters")]
        [DataType(DataType.PostalCode)]        
        public string PostalCode;

        [Required(ErrorMessage = "The Country is required")]       
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Country must have atleast 2 and upto 15 characters")]
        [RegularExpression(@"^([A-Za-z ]+)$", ErrorMessage = "Country is invalid")]
        public string Country;
        
        [StringLength(24, MinimumLength = 4, ErrorMessage = "Phone must have atleast 8 and upto 24 characters")]
        [DataType(DataType.PhoneNumber)]
        public string Phone;


        [StringLength(24, MinimumLength = 4, ErrorMessage = "Fax must have atleast 8 and upto 24 characters")]
        [DataType(DataType.PhoneNumber)]
        public string Fax;
    }
}