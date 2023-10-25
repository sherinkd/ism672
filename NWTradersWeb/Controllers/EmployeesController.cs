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
