using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NWTradersWeb.Models;

namespace NWTradersWeb.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.Shipper);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;

            if (currentCustomer == null)
            { return RedirectToAction("Login", "Customers"); }

            if (currentCustomer.myShoppingCart == null)
            { return RedirectToAction("Index", "Products"); }

            // Capture the order from the Current customer's shopping cart.
            Order order = currentCustomer.myShoppingCart;

            // Make sure there is a customer ID since we are not allowing edits to this field on the Create Page.
            order.CustomerID = currentCustomer.CustomerID;

            // Set some defaults for the Order and its dates - Allow the user to change the values in the Create Page.
            if (order.OrderDate == null)
                order.OrderDate = System.DateTime.Today;

            if (order.ShippedDate == null)
                order.ShippedDate = order.OrderDate.Value.AddDays(7);

            if (order.RequiredDate == null)
                order.RequiredDate = order.OrderDate.Value.AddDays(14);

            order.ShipName = currentCustomer.CompanyName;
            order.ShipAddress = currentCustomer.Address;
            order.ShipCity = currentCustomer.City;
            order.ShipRegion = currentCustomer.Region;
            order.ShipPostalCode = currentCustomer.PostalCode;
            order.ShipCountry = currentCustomer.Country;


            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");

            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;

            if (currentCustomer == null)
            { RedirectToAction("Login", "Customers"); }

            if (currentCustomer.myShoppingCart == null)
            { RedirectToAction("Index", "Products"); }

            // Have to add this since the user is not typing in the customer ID.
            order.CustomerID = currentCustomer.CustomerID;
            order.OrderDate = System.DateTime.Today;

            // The user has the order details in their shopping cart
            // Capture that information into the order that needs to be saved.
            order.Order_Details = currentCustomer.myShoppingCart.Order_Details;

            // Each order detail has a product object, to show information about the product bought.
            // This product is already in the DB - so we should NOT add it again
            // - otherwise it creates garbage data by adding duplicate products.
            // If we dont remove the product before saving,
            // the DB will think we have a new Product and
            // add garbage duplicate products to the database.
            // Note: We are not removing the product ID, just the Product Object.
            foreach (Order_Detail item in order.Order_Details)
            {
                item.Product = null;
            }

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                // After the changes are saved, clear the shopping cart.
                currentCustomer.myShoppingCart = null;

                return RedirectToAction("Details", "Customers", new { @id = currentCustomer.CustomerID });
                //return RedirectToAction("Index", "Products");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", order.ShipVia);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", order.ShipVia);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", order.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", order.ShipVia);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
