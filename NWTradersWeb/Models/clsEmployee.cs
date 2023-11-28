using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Models
{
    public partial class Employee
    {

        public string FullName
        { get { return FirstName + " " + LastName; } }


        public IEnumerable<OrderProducts> AnnualOrders()
        {
            List<OrderProducts> annualOrders = this.Orders.
                Where(od => od.OrderDate.Value.Year <= 2020).
                GroupBy(od => od.OrderDate.Value.Year).
                    Select(annual => new OrderProducts
                    {
                        DateNumber = annual.Key,
                        DateString = annual.Key.ToString(),
                        NumberOfOrders = annual.Count()
                    }).ToList();

            return annualOrders;
        }

        public IEnumerable<OrderProducts> AnnualOrdersInYear(string Year = "")
        {
            DateTime YearBeginning = NWTradersUtilities.BeginningOfYear(Year);
            DateTime YearEnd = NWTradersUtilities.EndOfYear(Year);


            List<OrderProducts> annualOrders = this.Orders.
                Where(od => od.OrderDate >= YearBeginning && od.OrderDate <= YearEnd).
                GroupBy(od => od.OrderDate.Value.Month).
                    Select(monthly => new OrderProducts
                    {
                        DateNumber = monthly.Key,
                        DateString = NWTradersUtilities.MonthName(monthly.Key),
                        NumberOfOrders = monthly.Count()
                    }).
                    ToList();

            List<string> monthsWithOrders = annualOrders.Select(o => o.DateString).ToList();
            List<string> AllMonths = NWTradersUtilities.AllMonthsNames();

            // Now add the empty months with a zero for Number Of Orders.
            for (int i = 0; i < AllMonths.Count(); i++)
            {
                if (monthsWithOrders.Contains(AllMonths.ElementAt(i)) == false)
                    annualOrders.Add(
                        new OrderProducts
                        {
                            DateNumber = i,
                            DateString = AllMonths.ElementAt(i),
                            NumberOfOrders = 0
                        });
            }

            return annualOrders.OrderBy(o => o.DateNumber);
        }


        public IEnumerable<OrderProducts> AnnualProducts()
        {
            List<OrderProducts> annualOrders = this.Orders.
                Where(od => od.OrderDate.Value.Year <= 2020).
                GroupBy(od => od.OrderDate.Value.Year).
                Select(annual => new OrderProducts
                {
                    DateNumber = annual.Key,
                    DateString = annual.Key.ToString(),
                    NumberOfOrders = annual.Sum(
                            o => o.Order_Details.Sum(od => od.Quantity))
                }).ToList();

            return annualOrders;
        }


        public IEnumerable<OrderRevenues> AnnualRevenues()
        {
            List<OrderRevenues> annualRevenues = this.Orders.
                Where(od => od.OrderDate.Value.Year <= 2020).
                GroupBy(od => od.OrderDate.Value.Year).
                    Select(annual => new OrderRevenues
                    {
                        Date = annual.Key.ToString(),
                        Sales = annual.Sum(o => o.TotalSales)
                    }).ToList();

            return annualRevenues;
        }

        public double AverageAnnualRevenues()
        {
            double averageAnnualRevenues = 0D;

            averageAnnualRevenues = this.Orders.SelectMany(o => o.Order_Details).
                Where(od => od.Order.OrderDate.Value.Year <= 2020).
                GroupBy(od => od.Order.OrderDate.Value.Year).
                Select(
                ods =>
                    ods.Sum(od =>
                        ((double)od.UnitPrice - ((double)od.UnitPrice * (double)od.Discount)) * od.Quantity)).
                    Average();
            return averageAnnualRevenues;
        }

        public decimal AnnualSales(string Year = "")
        {
            decimal annualSales = 0;

            if (string.IsNullOrEmpty(Year))
                annualSales = Orders.Sum(od => od.TotalSales);
            else
                annualSales = Orders.
                    Where(od =>
                        od.OrderDate > NWTradersUtilities.BeginningOfYear(Year) &&
                        od.OrderDate < NWTradersUtilities.EndOfYear(Year)
                     ).
                    Sum(od => od.TotalSales);


            return annualSales;

        }

        public List<BestSellingProduct> BestSellingProducts(int limit)
        {
            return Orders
                .SelectMany(s => s.Order_Details)
                .Select(od => od.Product)
                .GroupBy(s => s.ProductName, s => s.ProductID, (key, g) => new BestSellingProduct { ProductName = key, NumberOfSales = g.ToList().Count() })
                .OrderByDescending(s => s.NumberOfSales)
                .Take(limit)
                .ToList();
        }

        public List<BestCustomer> BestCustomers(int limit)
        {
            return Orders
                .Select(s => s.Customer)
                .GroupBy(s => s.CompanyName, s => s.CustomerID, (key, g) => new BestCustomer { CompanyName = key, NumberOfOrders = g.ToList().Count() })
                .OrderByDescending(s => s.NumberOfOrders)
                .Take(limit)
                .ToList();
        }
    }

    public class BestSellingProduct
    {
        public string ProductName { get; set; }
        public int NumberOfSales { get; set; }
    }

    public class BestCustomer
    {
        public string CompanyName { get; set; }
        public int NumberOfOrders { get; set; }
    }
}