using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{

    /// <summary>
    /// The "MetaData" Class is introduced here for illustrative purposes ... 
    /// We will work a lot more on this class in Assignment 2. 
    /// Here we are only using this to illustrate the concept.
    /// </summary>
    public class SupplierMetaData
    {

        [Display(Name = "Product Supplier")]
        public string CompanyName { get; set; }

    }

    [MetadataType(typeof(SupplierMetaData))]
    public partial class Supplier
    {
    }
}