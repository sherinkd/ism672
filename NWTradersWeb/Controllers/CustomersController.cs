/*
 * Every class or page will typically begin by stating the sets of functions that it needs to use to perform its functions.
 * These are expressed in the "using" statements.
 * Most commonly, the usings are the 
 */

using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using NWTradersWeb.Models;


/// A namespace is like a folder or a path.
/// So - we are saying here that all are controllers - current and future,
/// will be in the "NWTradersWeb.Controllers" Folder or have this path.
/// Typically, the Root Namespace is the name of the solution - "NWTradersWeb"
/// Then sub-folders or sub-paths are placed in Root Namespace.
/// From another perspective, a Namespace is a collection of classes. 
/// So - NWTradersWeb.Controllers is the collection of all the Controllers in the Application.
/// ToDo: Review the other controllers and you will see the same namespace that the controllers are placed in.
namespace NWTradersWeb.Controllers
{
    /// <summary>
    /// The CustomersController is the class that manages objects of the type - Customer; 
    /// as well as provide data for the web pages that relate to the Customer class.
    /// It manages access to the database using the "NorthwindEntities" object called "nwEntities"
    /// So - it acts the bridge between the database, the model (in this case the Customer Class), and the View Pages.
    /// In this sense, it represents the go-between, middle tier or the "middle-ware" that provides managed access to data.
    /// 
    /// It "derives" from an MVC Contoller - hence the ": Controller". 
    /// </summary>
    public class CustomersController : Controller
    {

        /// <summary>
        /// nwEntities is the name of the object of type NorthwindEntities and it is your handle to the database.
        /// It provides direct Read/Write access to all the data in the Northwind Database and 
        /// uses the relationships in the model to provide the access.
        /// 
        /// nwEntities is the instantiation of the Entity Framework in your application. 
        /// By making it private, we are not allowing anyone (object or function) to get access to the database.
        /// Therefore, the only way access to data is provided is by requesting the controller.
        /// The controller brokers the request, typically from the view, gets the data from the DB 
        /// and provides this "managed" access to the requestor - typically the Views.
        /// 
        /// </summary>
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
            string searchCompanyName = "", // if searchCompany name is , we are searching by CompanyName.
            string searchCountryName = "", // if searchCountry is NOT provided, we are NOT searching by Country Name.
            string searchTitle = "")
        {

            //if(string.IsNullOrEmpty(searchCompanyName) && 
            //    string.IsNullOrEmpty(searchCountryName) &&
            //    string.IsNullOrEmpty(searchTitle))
            //{
            //    return View(new List<Customer>());
            //}

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
                    //Also - note the "Contains" will look for the search string in any place.
                    // Experiment with ... StartsWith instead of contains.
                    theCustomers = theCustomers.
                        Where(c => c.CompanyName.ToUpper().Contains(searchCompanyName.ToUpper())).
                        OrderBy(c => c.CompanyName).
                        Select(c => c);
                }
            ViewBag.searchCompanyName = searchCompanyName;


            //Since the value comes from the dropdown, we know the name will be exact - 
            // no typos are possible since the user is not typing.
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
                    // This is an easy and equally effective way to manage case-insensitive searches.
                    theCustomers = theCustomers.
                        Where(c => c.ContactTitle.ToUpper().Contains(searchTitle.ToUpper())).
                        OrderBy(c => c.CompanyName).
                        Select(c => c);
                }
            ViewBag.searchTitle = searchTitle;

            return View(theCustomers);
        }

        // GET: Customers/Details/5
        /// <summary>
        /// The index function returns an actionResult (typically a page) which shows a list of objects, 
        /// the details function returns an actionResult (typically a page) which shows ONE object, 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// The above functions (Index and Details) are "Read" functions.
        /// They do not change the data in any way.
        /// They are called "Get" functions - in that they supply a View of the data.
        /// 
        /// The functions (below) here are responsible for both reading and writing data. 
        /// They come in "Get/Post" pairs - 
        /// Get functions to show a page with current (or empty) data. 
        /// Post functions bring the data from the page (form) and write (POST) that data to the DB.
        /// 
        /// Get functions do very little beyond finding an object and returning the data to the page.
        /// Post functions collect the data from the page (Data that is "bound") in the form.
        /// They then update the object and send it to "POST" to the database
        /// using the provided by the EF Object - nwEntities
        /// </summary>

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
