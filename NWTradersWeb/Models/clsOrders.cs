using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NWTradersWeb.Models
{
    /// <summary>
    /// A customer can place many orders. 
    /// Every order contains (or is associated with the multiple Order_Details (line Items) on that order.
    /// 
    /// To Note: 
    /// Why is it a "Public" Class?
    /// Why is this a partial class?
    /// 
    /// </summary>
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {

        /// <summary>
        /// The total for the order is calculated as the sum of order_Detail Total 
        /// for every order_detail on (associated with) the Order.
        /// </summary>
        public decimal orderTotal
        {
            get
            { return this.Order_Details.Sum(od => od.Total); }
        }

        public decimal TotalSales
        {
            get
            { return this.Order_Details.Sum(od => od.Total); }
        }


        /// <summary>
        ///  Get all the Products that were ordered.
        /// </summary>
        /// <returns>A List of Products</returns>
        public List<Product> OrderedProducts()
        {
            return this.
                Order_Details.
                Select(od => od.Product).
                ToList();
        }

        /// <summary>
        /// To Add a new Product to "theCurrentOrder" (Shopping Cart)
        /// 1. The Product must not be Discontinued.
        /// 2. Manage Adding Products already in the Order - Increase Quantity
        /// 3. Manage Adding a Product in an order that does not contain the product
        /// </summary>
        /// <returns></returns>
        public bool AddToOrder(Product theProductToAdd, short QuantityOrdered = 1)
        {
            //1. The Product must not be Discontinued.
            if (theProductToAdd.Discontinued)
                return false;

            //2. May be the order already has this product??
            bool AlreadyBought = (this.OrderedProducts().Contains(theProductToAdd, new ProductComaprer()));

            // If true, then the order already contains this product.
            if (AlreadyBought)
            {
                Order_Detail TheOneWiththeProductToAdd = this.
                    Order_Details.
                    Where(od => od.ProductID == theProductToAdd.ProductID).
                    FirstOrDefault();

                // Now we know while "line item" to add this to.
                TheOneWiththeProductToAdd.Quantity += QuantityOrdered;
                return true;
            }
            // 3. The product is available (Not Discontinued) && The order does not contain the product.
            else // the product is being added for the first time.
            {
                Order_Detail NewOrderDetail = new Order_Detail();
                NewOrderDetail.Product = theProductToAdd;
                NewOrderDetail.ProductID = theProductToAdd.ProductID;

                NewOrderDetail.UnitPrice = theProductToAdd.UnitPrice.Value;
                NewOrderDetail.Quantity = QuantityOrdered;

                this.Order_Details.Add(NewOrderDetail);
                return true;
            }

        }

        public bool RemoveAllFromCart()
        {
            this.Order_Details.Clear();
            return true;
        }

        public bool RemoveFromOrder(Product theProductToRemove, short QuantityToRemove = 1)
        {
            Order_Detail TheOneWiththeProductToAdd = this.
                    Order_Details.
                    Where(od => od.ProductID == theProductToRemove.ProductID).
                    FirstOrDefault();

            // Now we know while "line item" to add this to.
            TheOneWiththeProductToAdd.Quantity -= QuantityToRemove;

            if (TheOneWiththeProductToAdd.Quantity == 0) {
                this.Order_Details = this.Order_Details.Where(od => od.ProductID != theProductToRemove.ProductID).ToList();
            }

            return true;
        }

        public bool RemoveProductFromOrder(Product theProductToRemove)
        {
            this.Order_Details = this.Order_Details.Where(od => od.ProductID != theProductToRemove.ProductID).ToList();
            return true;
        }
    }


}
