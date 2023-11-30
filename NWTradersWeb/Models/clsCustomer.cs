using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NWTradersWeb.Models
{

    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer
    {
        #region Analysis Functions

        #region Customer Annual Sales

        public IEnumerable<CustomerSales> CustomerAnnualSales()
        {
            List<CustomerSales> customerSales = new List<CustomerSales>();


            customerSales = this.Orders.
                GroupBy(o => o.OrderDate.Value.Year).
                Select(ordersInYear =>
                new CustomerSales
                {
                    theCustomer = this.CompanyName,
                    SalesPeriod = ordersInYear.Key.ToString(),
                    NumberOfOrders = ordersInYear.Count(),
                    Sales = ordersInYear.Sum(o => o.TotalSales)
                    //SelectMany(od => od.Order_Details).
                    //Sum(od => od.Quantity*od.UnitPrice)
                }).ToList();
            return customerSales;
        }

        public IEnumerable<CustomerSales> CustomerSalesInYear(string Year)
        {
            DateTime YearBeginning = NWTradersUtilities.BeginningOfYear(Year);
            DateTime YearEnd = NWTradersUtilities.EndOfYear(Year);

            List<CustomerSales> customerSales = new List<CustomerSales>();

            customerSales = this.Orders.
                Where(od => od.OrderDate >= YearBeginning && od.OrderDate <= YearEnd).
                GroupBy(o => o.OrderDate.Value.Month).
                Select(ordersInMonth =>
                new CustomerSales
                {
                    theCustomer = this.CompanyName,
                    SalesPeriod = NWTradersUtilities.MonthName(ordersInMonth.Key),
                    SalesPeriodNumber = ordersInMonth.Key,
                    NumberOfOrders = ordersInMonth.Count(),
                    Sales = ordersInMonth.Sum(o => o.TotalSales)
                    //SelectMany(od => od.Order_Details).
                    //Sum(od => od.Quantity*od.UnitPrice)
                }).ToList();

            List<string> monthsWithOrders = customerSales.Select(o => o.SalesPeriod).ToList();
            List<string> AllMonths = NWTradersUtilities.AllMonthsNames();

            // Now add the empty months with a zero for Number Of Orders.
            for (int i = 0; i < AllMonths.Count(); i++)
            {
                if (monthsWithOrders.Contains(AllMonths.ElementAt(i)) == false)
                    customerSales.Add(
                        new CustomerSales
                        {
                            theCustomer = this.CompanyName,
                            SalesPeriod = AllMonths.ElementAt(i),
                            SalesPeriodNumber = i,
                            NumberOfProducts = 0,
                            Sales = 0,
                            NumberOfOrders = 0
                        });
            }

            return customerSales.OrderBy(c => c.SalesPeriodNumber);
        }



        #endregion

        #region Top Products

        public IEnumerable<ProductSales> CustomerSalesInYear(int? Year)
        {
            return this
                .Orders
                .Where(od => od.OrderDate.Value.Year == Year || (Year == null && od.OrderDate.Value.Year <= DateTime.Now.Year))
                .SelectMany(od => od.Order_Details)
                .Where(od => !od.Product.Discontinued)
                .GroupBy(od => od.Product)
                .Select(ps => new ProductSales {
                    ProductName = ps.Key.ProductName,
                    Sales = ps.Sum(s => s.Quantity * s.UnitPrice)
                })
                .ToList();
                
        }

        #endregion Top Products

        #region Top Product Categories

        public IEnumerable<ProductCategorySales> TopProductCategories(int? Year)
        {
            return this
                .Orders
                .Where(od => od.OrderDate.Value.Year == Year || (Year == null && od.OrderDate.Value.Year <= DateTime.Now.Year))
                .SelectMany(od => od.Order_Details)
                .Where(od => !od.Product.Discontinued)
                .GroupBy(od => od.Product.Category.CategoryName)
                .Select(ps => new ProductCategorySales
                {
                    ProductCategory = ps.Key,
                    Sales = ps.Sum(s => s.Quantity * s.UnitPrice)
                })
                .ToList();

        }

        #endregion Top Product Categories

        /// <summary>
        /// Returns the date when the last order was placed by a particular customer. 
        /// If no orders are placed, the function returns a <see langword="null"/> DateTime Object
        /// </summary>
        /// <returns></returns>
        public DateTime? LastOrderPlacedOn()
        {
            DateTime? lastOrderPlacedOn = null;

            if (Orders.Count() > 0)
            lastOrderPlacedOn=    Orders.
                       Where(o => o.OrderDate.HasValue).
                       OrderByDescending(o => o.OrderDate).
                       Select(o => o.OrderDate.Value).
                       FirstOrDefault();
            return lastOrderPlacedOn;
        }

        public List<Order> RecentOrders(int limit) { 
            return Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(limit)
                .ToList();
        }

        public List<Product> RecentlyPurchasedProducts(int limit)
        {
            return Orders
                .OrderByDescending(o => o.OrderDate)
                .SelectMany(o => o.Order_Details)
                .Select(od => od.Product)
                .Distinct()
                .Take(limit)
                .ToList();
        }


        public bool isCustomerIDNotUnique;

        public Order myShoppingCart;

        public Boolean AddProductToCart(Product theProductToAdd, short howMany = 1)
        {
            if (myShoppingCart == null)
            {
                myShoppingCart = new Order();
                myShoppingCart.Customer = this;
                myShoppingCart.CustomerID = this.CustomerID;
                myShoppingCart.OrderDate = System.DateTime.Today;
            }

            return this.myShoppingCart.AddToOrder(theProductToAdd, howMany);
        }

        public Boolean RemoveProductFromCart(Product theProductToRemove, short howMany = 1)
        {
            return this.myShoppingCart.RemoveFromOrder(theProductToRemove, howMany);
        }

        public Boolean RemoveAllFromCart()
        {
            return myShoppingCart.RemoveAllFromCart();
        }

        public Boolean RemoveProductFromCart(Product theProductToRemove)
        {
            return this.myShoppingCart.RemoveProductFromOrder(theProductToRemove);
        }
    }
    #endregion
}