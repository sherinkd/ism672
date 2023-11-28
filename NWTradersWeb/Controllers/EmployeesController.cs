/*
 * Every class or page will typically begin by stating the sets of functions that it needs to use to perform its functions.
 * These are expressed in the "using" statements.
 * Most commonly, the usings are the 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using NWTradersWeb.Models;
using NWTradersWeb.Utilities;

namespace NWTradersWeb.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities nwEntities = new NorthwindEntities();

        public ActionResult Login(string EmployeeName = "")
        {
            Session["currentEmployee"] = null;
            Session.Clear(); 
            Session.Abandon();

            ViewBag.EmployeeName = EmployeeName;
            ViewBag.Message = "Welcome to NW Traders . Please Login with your Employee Name and Password.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(int EmployeeID, string Password)
        {
            if (EmployeeID <= 0 || string.IsNullOrEmpty(Password))
            {
                ViewBag.EmployeeIDMessage = "Please enter Employee ID and Password";
                return View();
            }

            Employee employee = nwEntities.Employees
                .Where(c => c.EmployeeID == EmployeeID && c.Password == Password)
                .Select(c => c)
                .FirstOrDefault();
            Session["currentEmployee"] = employee;
            if (employee == null)
            {
                ViewBag.EmployeeIDMessage = "The employee ID you entered is not valid. Please enter a valid Employee ID as your password";
                return View();
            }

            return RedirectToAction("Details", "Employees", new { @id = employee.EmployeeID });
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = nwEntities.Employees.Find(Int32.Parse(id));
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult Analysis(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = nwEntities.Employees.Find(Int32.Parse(id));
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        #region Utilities

        public double CompanyAverageAnnualOrders()
        {
            double averageAnnualOrders = 0D;

            averageAnnualOrders = nwEntities.
                Orders.
                Where(o => o.OrderDate.Value.Year <= 2020).
                GroupBy(empOrder => new
                {
                    emp = empOrder.EmployeeID,
                    Year = empOrder.OrderDate.Value.Year
                }).
                    Select(avgOrder => new
                    {
                        nOrders = avgOrder.Count()
                    }).
                    Average(o => o.nOrders);


            return averageAnnualOrders;
        }

        public double EmployeeAverageAnnualOrders(int? EmployeeID)
        {
            double averageAnnualOrders = 0D;

            if ((EmployeeID == null) || (nwEntities.Employees.Find(EmployeeID) == null))
                return averageAnnualOrders;

            averageAnnualOrders = nwEntities.
                Orders.
                Where(o => o.EmployeeID == EmployeeID).
                Where(o => o.OrderDate.Value.Year <= 2020).
                GroupBy(empOrder => new
                {
                    emp = empOrder.EmployeeID,
                    Year = empOrder.OrderDate.Value.Year
                }).
                    Select(avgOrder => new
                    {
                        nOrders = avgOrder.Count()
                    }).
                    Average(o => o.nOrders);


            return averageAnnualOrders;
        }

        public List<ProductSales> CompanyTop10Products()
        {
            return nwEntities.Order_Details.
                    Where(od => (od.Product.Discontinued == false)).
                    GroupBy(od => od.Product.ProductName).
                    Select(prodSales => new ProductSales()
                    {
                        ProductName = prodSales.Key,
                        Sales = prodSales.Sum(od => od.Quantity * od.UnitPrice)
                    }).
                    OrderByDescending(pSale => pSale.Sales).
                    Take(10).
                    ToList();
        }

        public List<CustomerSales> AllEmployeeTop10Customers()
        {

            return nwEntities.Order_Details.
                    Where(od => (od.Product.Discontinued == false)).
                    GroupBy(od => od.Order.Customer.CompanyName).
                    Select(custSales => new CustomerSales()
                    {
                        theCustomer = custSales.Key,
                        Sales = custSales.Sum(od => od.Quantity * od.UnitPrice)
                    }).
                    OrderByDescending(pSale => pSale.Sales).
                    Take(10).
                    ToList();
        }

        public decimal CompanyTotalRevenues()
        {
            return nwEntities.Order_Details.Sum(od => od.UnitPrice * od.Quantity);
        }

        public decimal EmployeeTotalRevenues(int? EmployeeID)
        {
            if (EmployeeID == null || nwEntities.Employees.Find(EmployeeID) == null)
                return 0;

            return nwEntities.Order_Details.
                Where(od => od.Order.EmployeeID == EmployeeID).
                Sum(od => od.UnitPrice * od.Quantity);
        }

        public double AllEmployeesAverageAnnualRevenues()
        {
            double averageAnnualRevenues = 0D;

            averageAnnualRevenues = nwEntities.Order_Details.
                GroupBy(od => new
                {
                    emp = od.Order.EmployeeID,
                    year = od.Order.OrderDate.Value.Year
                }).
                Select(
                ods =>
                    ods.Sum(od =>
                        ((double)od.UnitPrice - ((double)od.UnitPrice * (double)od.Discount)) * od.Quantity)).
                    Average();

            return averageAnnualRevenues;
        }

        public double EmployeeAverageAnnualRevenues(int? EmployeeID)
        {
            double averageAnnualRevenues = 0D;

            if ((EmployeeID == null) || (nwEntities.Employees.Find(EmployeeID) == null))
                return averageAnnualRevenues;


            averageAnnualRevenues = nwEntities.Order_Details.
                Where(od => od.Order.EmployeeID.Equals(EmployeeID)).
                GroupBy(od => new
                {
                    year = od.Order.OrderDate.Value.Year
                }).
                Select(
                ods =>
                    ods.Sum(od =>
                        ((double)od.UnitPrice - ((double)od.UnitPrice * (double)od.Discount)) * od.Quantity)).
                    Average();

            return averageAnnualRevenues;
        }

        #endregion

        #region AllEmployeeSales

        public ActionResult AllEmployeeSales(string Year = "All Years")
        {
            Employee theEmployee = Session["currentEmployee"] as Employee;
            if (theEmployee == null)
                return RedirectToAction("Login");

            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);

            ViewBag.Year = Year;
            ViewBag.YearsList = NWTradersUtilities.BeginToEndOrderYears();

            return PartialView("_AllEmployeeSales", new List<EmployeeSales>());
        }

        public JsonResult GetAllEmployeeSales(string Year = "All Years")
        {

            IEnumerable<EmployeeSales> allEmployeeSales = new List<EmployeeSales>();

            if (string.IsNullOrEmpty(Year) || Year.Equals("All Years"))
            {
                allEmployeeSales = nwEntities.Orders.
                    GroupBy(order => new { order.Employee.FirstName, order.Employee.LastName }).
                    OrderBy(a => a.Key.LastName).
                    Select(empSales => new EmployeeSales
                    {
                        theEmployee = empSales.Key.FirstName + " " + empSales.Key.LastName,
                        Sales = empSales.Sum(o => o.Order_Details.Sum(
                            od => od.Quantity * od.UnitPrice))
                    });
            }
            else
            {
                DateTime begin = NWTradersUtilities.BeginningOfYear(Year);
                DateTime end = NWTradersUtilities.EndOfYear(Year);

                allEmployeeSales = nwEntities.Orders.
                    Where(order => (order.OrderDate >= begin) && (order.OrderDate <= end)).
                    GroupBy(order => new { order.Employee.FirstName, order.Employee.LastName }).
                    OrderBy(a => a.Key.LastName).
                    Select(empSales => new EmployeeSales
                    {
                        theEmployee = empSales.Key.FirstName + " " + empSales.Key.LastName,
                        Sales = empSales.Sum(o => o.Order_Details.Sum(
                            od => od.Quantity * od.UnitPrice))
                    });
            }

            return Json(new { JSONList = allEmployeeSales }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Annual Orders

        public ActionResult AnnualOrders()
        {
            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);
            IEnumerable<OrderProducts> annualOrders = theEmployee.AnnualOrders();

            ViewBag.CompanyAverageAnnualOrders = CompanyAverageAnnualOrders();
            ViewBag.EmployeeAverageAnnualOrders = EmployeeAverageAnnualOrders(theEmployee.EmployeeID);

            return PartialView("_AnnualOrders", annualOrders);
        }

        public JsonResult GetAnnualOrders()
        {
            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);
            IEnumerable<OrderProducts> annualOrders = theEmployee.AnnualOrders();

            return Json(new { JSONList = annualOrders }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnnualOrdersModal(string Year = "")
        {
            int year = DateTime.Now.Year;

            if (string.IsNullOrEmpty(Year))
                year = int.Parse(Year);
            else
                year = int.Parse(Year);

            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);
            IEnumerable<OrderProducts> annualOrders = theEmployee.AnnualOrdersInYear(Year);

            ViewBag.Year = year;
            return PartialView("_AnnualOrdersModal", annualOrders);
        }

        public JsonResult GetAnnualOrdersModal(string Year = "")
        {

            int year = DateTime.Now.Year;

            if (string.IsNullOrEmpty(Year))
                year = int.Parse(Year);

            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);
            IEnumerable<OrderProducts> annualOrders = theEmployee.AnnualOrdersInYear(Year);

            return Json(new { JSONList = annualOrders }, JsonRequestBehavior.AllowGet);


        }

        #endregion

        #region Annual Revenues

        public ActionResult AnnualRevenues()
        {
            Employee currentEmployee = Session["currentEmployee"] as Employee;
            currentEmployee = nwEntities.Employees.Find(currentEmployee.EmployeeID);
            IEnumerable<OrderRevenues> annualRevenues = currentEmployee.AnnualRevenues();

            ViewBag.CompanyAverageAnnualRevenues = AllEmployeesAverageAnnualRevenues();
            //ViewBag.EmployeeAverageAnnualRevenues = EmployeeAverageAnnualRevenues(theEmployee.EmployeeID);
            ViewBag.EmployeeAverageAnnualRevenues = currentEmployee.AverageAnnualRevenues();
            return PartialView("_AnnualRevenues", annualRevenues);
        }

        public JsonResult GetAnnualRevenues()
        {
            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);
            IEnumerable<OrderRevenues> annualRevenues = theEmployee.AnnualRevenues();

            return Json(new { JSONList = annualRevenues }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAnnualRevenues()
        {

            int year = DateTime.Now.Year;

            Employee theEmployee = Session["currentEmployee"] as Employee;
            theEmployee = nwEntities.Employees.Find(theEmployee.EmployeeID);

            IEnumerable<OrderRevenues> allannualRevenues = nwEntities.Orders.
                GroupBy(oo => oo.OrderDate.Value.Year).
                Select(annual => new OrderRevenues
                {
                    Date = annual.Key.ToString(),
                    Sales = annual.SelectMany(o => o.Order_Details).
                                        Sum(oo => oo.UnitPrice * oo.Quantity)
                }).
                OrderBy(ans => ans.Date).
                ToList();

            return Json(new { JSONList = allannualRevenues }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                nwEntities.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
