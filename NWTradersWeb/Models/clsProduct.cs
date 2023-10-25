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

    class ProductComaprer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            //Check whether the Product.ProductIDare equal.
            return (x.ProductID == y.ProductID);
        }

        public int GetHashCode(Product product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null))
            {
                return 0;
            }

            //Get hash code for the Name field if it is not null.
            int hashProjDetail = product.ProductID.GetHashCode();

            return hashProjDetail;
        }
    }
}