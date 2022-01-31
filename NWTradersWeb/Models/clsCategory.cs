using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{

    public class CategoryMetaData
    {
        [Display(Name = "Product Category ")]
        public string CategoryName { get; set; }
    }

    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }
}