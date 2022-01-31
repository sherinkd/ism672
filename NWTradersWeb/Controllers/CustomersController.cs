using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using NWTradersWeb.Models;

namespace NWTradersWeb.Controllers
{
    public class CustomersController : Controller
    {
        private NorthwindEntities nwEntities = new NorthwindEntities();

        // GET: Customers
        /// <summary>
        /// Function provides the data for the "Index" page in the Customers Folder. 
        /// It following the MVC convention: ControllerName -> Folder Name; ActionResult FunctionName -> Page Name.
        /// This function is called when the user clicks on a link to "http://YourWebServer:Port#/Customers/Index"
        /// Any arguments supplied are used and passed to the function, otherwise default values provided in the signature are used.
        /// The function returns a view of the same name as the function name found in the folder with the same name as the controller name.
        /// The function supplies the "Model" for the page ... the data that is to be displayed.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchCompanyName"></param>
        /// <param name="searchCountryName"></param>
        /// <param name="searchTitle"></param>
        /// <returns></returns>
        public ActionResult Index(
            int? page, // which page # 
            string itemsPerPage = "15", // the number of items (customers) to display on each page.
            string sortOrder = "", // A sortby field - empty (by default ) - means that a default sort order is applied.
            string searchCompanyName = "", // if searchCompany name is not provided, we are NOT searching by CompanyName.
            string searchCountryName = "", // if searchCountry is provided, we ARE searching by Country Name.
            string searchTitle="")
        {

            // begin by getting all the customers from the db
            IEnumerable<Customer> theCustomers = nwEntities.Customers.
                OrderBy(c => c.CompanyName).
                Select(c => c).ToList();


            if (theCustomers.Count() > 0)
                // Here the ignore case allows for searches that are not case sensitive.
                // Use this to do case insensitive searches for any field.
                if (string.IsNullOrEmpty(searchCompanyName) == false)
                {
                    // Here the ignore case allows for searches that are not case sensitive.
                    // Use this to do case insensitive searches for any field.
                    theCustomers = theCustomers.
                        Where(c => c.CompanyName.StartsWith(searchCompanyName, ignoreCase: true, new System.Globalization.CultureInfo("en-US"))).
                        OrderBy(c => c.CompanyName).
                        Select(c => c);
                }
            ViewBag.searchCompanyName = searchCompanyName;


            if (theCustomers.Count() > 0)
                if (string.IsNullOrEmpty(searchCountryName) == false)
                {
                    theCustomers = theCustomers.
                        Where(c => c.Country.Equals(searchCountryName)).
                        OrderBy(c => c.CompanyName).
                        Select(c => c);
                }
            ViewBag.searchCountryName = searchCountryName;

            if (theCustomers.Count() > 0)
                if (string.IsNullOrEmpty(searchTitle) == false)
                {
                    theCustomers = theCustomers.
                        Where(c => c.ContactTitle.Contains(searchTitle)).
                        OrderBy(c => c.CompanyName).
                        Select(c => c);
                }
            ViewBag.searchTitle = searchTitle;


            switch (sortOrder)
            {
                case "CompName_desc":
                    theCustomers = theCustomers.
                        Where(c => c.CompanyName.StartsWith(searchCompanyName)).
                        OrderBy(c => c.CompanyName);
                    break;

                default: //All - do nothing
                    break;

            }

            // page size is supplied 
            int pageSize;
            if (itemsPerPage == "All")
                pageSize = theCustomers.Count();
            else
                // If the user provides a selection for items per page, then read that string as an integer into pageSize
                pageSize = (int.Parse(itemsPerPage));

            int pageNumber = (page ?? 1);

            ViewBag.itemsPerPage = itemsPerPage;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return View(theCustomers.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = nwEntities.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }



        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                nwEntities.Customers.Add(customer);
                nwEntities.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }




        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = nwEntities.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                nwEntities.Entry(customer).State = EntityState.Modified;
                nwEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = nwEntities.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = nwEntities.Customers.Find(id);
            nwEntities.Customers.Remove(customer);
            nwEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        ///// <summary>
        ///// The Current Customer (synchronized with the Session "currentCustomer") is the customer currently logged in.
        ///// If the Current Customer is null, then no actions are allowed.
        ///// </summary>
        public ActionResult Login(string CompanyName = "")
        {
            //Clear the current customer, if one is already logged in 
            Session["currentCustomer"] = null;
            Session.Clear(); // Clear the session
            Session.Abandon(); // And start a new one.

            ViewBag.CompanyName = CompanyName;

            ViewBag.Message = "Welcome to NW Traders . Please Login with your Company Name and Customer ID .";
            return View();
        }

        /// <summary>
        /// Login the customer 
        /// </summary>
        /// <param name="CompanyName: We are using the company name of the customer."></param>
        /// <param name="CustomerID: We are using the CustomerID for the Customer."></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string CompanyName, string CustomerID)
        {

            if (string.IsNullOrEmpty(CompanyName))
            {
                ViewBag.CompanyNameMessage = "Please select your company";
                return View();
            }

            if (string.IsNullOrEmpty(CustomerID))
            {
                ViewBag.CustomerIDMessage = "Please Enter a Valid Customer ID as your password";
                return View();
            }

            // Here only if both Company Name and Customer ID are valid (neither null nor empty)

            // May need to convert case (toLower or toUpper)
            Customer currentCustomer = nwEntities.Customers. // From Customers.
                Where(c => c.CompanyName.Equals(CompanyName)). // Where the company name 
                Where(c => c.CustomerID.Equals(CustomerID)). // & Customer ID match,
                Select(c => c). // Select the customer
                FirstOrDefault(); // And get the first - there should be only one anyway.

            //Set the current user object for the session to TheCurrentUser (maybe null) 
            Session["currentCustomer"] = currentCustomer;
            if (currentCustomer == null)
            {
                ViewBag.CustomerIDMessage = "The customer ID you entered is not valid. Please enter a valid Customer ID as your password";
                return View();
            }
            else
                // the user is found, so load the selected customer informaiton.
                return RedirectToAction("Details", "Customers", new { @id = currentCustomer.CustomerID });
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
