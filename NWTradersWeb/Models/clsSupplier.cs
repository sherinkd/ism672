using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{

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