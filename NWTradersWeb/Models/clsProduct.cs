using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NWTradersWeb.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        public List<string> GetSoldRegions() {
            return Order_Details
                .Select(od => od.Order.ShipRegion)
                .Where(region => !string.IsNullOrEmpty(region) && !string.IsNullOrWhiteSpace(region))
                .OrderBy(region => region)
                .Distinct()
                .ToList();
        }

        public List<string> GetSoldEmployees()
        {
            return Order_Details
                .Select(od => od.Order.Employee)
                .Select(employee => employee.LastName + ", " + employee.FirstName)
                .OrderBy(employee => employee)
                .Distinct()
                .ToList();
        }
    }
}