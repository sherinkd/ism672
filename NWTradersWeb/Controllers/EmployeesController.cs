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

        #region AllEmployeeSales

        public ActionResult AllEmployeeSales(string Year = "All Years")
        {
            Employee theEmployee = Session["currentEmployee"] as Employee;
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
