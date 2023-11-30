using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Models
{
    public class OrderProducts
    {
        public int DateNumber;
        public string DateString;
        public int NumberOfOrders;
    }

    public class ProductSale {
        public string ProductName;
        public string ProductCategory;
        public decimal Sales;
    }

    public class CustomerRevenue
    {
        public string CustomerName;
        public string DateString;
        public decimal Sales;
    }

    public class OrderRevenues
    {
        public string Date;
        public decimal Sales;
    }

    public class EmployeeSales
    {
        public string theEmployee;
        public decimal Sales;
    }

    public class ProductSales
    {
        public string ProductName;
        public string ProductCategory;
        public string Year;
        public decimal Sales;
    }

    public class ProductCategorySales
    {
        public string ProductCategory;        
        public decimal Sales;
    }

    public class ProductCategoryRevenue
    {
        public string Year;
        public decimal Sales;
    }

    public class ProductCategoryOrder
    {
        public string Year;
        public int NumberOfOrders;
    }

    public class CustomerSales
    {
        public string theCustomer;
        public string SalesPeriod;
        public int SalesPeriodNumber;
        public decimal Sales;
        public int NumberOfOrders;
        public int NumberOfProducts;
    }

}