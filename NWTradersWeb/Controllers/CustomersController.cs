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
            string searchTitle = "",
            string searchContact = "",
            string searchRegion = "",
            string searchCity = "")
        {

            List<Customer> theCustomers = nwEntities.Customers.
                OrderBy(c => c.CompanyName).
                Select(c => c).ToList();

            if (theCustomers.Count() == 0)
            {
                return View(theCustomers);
            }


            ViewBag.searchCompanyName = searchCompanyName;
            ViewBag.searchCountryName = searchCountryName;
            ViewBag.searchTitle = searchTitle;
            ViewBag.searchContact = searchContact;
            ViewBag.searchRegion = searchRegion;
            ViewBag.searchCity = searchCity;


            return View(
                new CustomerSearchUtil(
                    theCustomers
                )
                .ByCompanyName(searchCompanyName)
                .ByCountryName(searchCountryName)
                .ByTitle(searchTitle)
                .ByContact(searchContact)
                .ByRegion(searchRegion)
                .ByCity(searchCity)
                .GetCustomers()
                );
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
            customer.isCustomerIDNotUnique = true;
            if (ModelState.IsValid)
            {

                if (nwEntities.Customers.Any(c => c.CustomerID == customer.CustomerID)) {
                    customer.isCustomerIDNotUnique = false;
                    return View(customer);
                }

                customer.CustomerID = customer.CustomerID.TrimEnd().TrimStart();
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

        #region Shopping Cart functions

        public ActionResult ShoppingCart(string customerId)
        {
            if (customerId == null)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Customer theCustomer = nwEntities.Customers.Find(customerId);

            if (theCustomer == null) { return HttpNotFound(); }

            return PartialView("_ShoppingCart", theCustomer);
        }

        public ActionResult AddToCart(int? ProductID = null)
        {

            if (ProductID == null)
                return RedirectToAction("Index", "Products");

            // If the current customer is Null - the customer is not logged in - 
            // Redirect them to the Login page for customer.
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login", "Customers");

            // Get the Product
            Product productToAdd = nwEntities.Products.Find(ProductID);
            if (productToAdd == null)
                return RedirectToAction("Index", "Products");

            bool success = currentCustomer.AddProductToCart(productToAdd);

            return RedirectToAction("Index", "Products");

        }

        public ActionResult RemoveProductFromCart(int? ProductID = null)
        {
            if (ProductID == null)
                return RedirectToAction("Index", "Products");

            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login", "Customers");

            Product productToRemove = nwEntities.Products.Find(ProductID);
            if (productToRemove == null)
                return RedirectToAction("Index", "Products");

            currentCustomer.RemoveProductFromCart(productToRemove);

            return RedirectToAction("Index", "Products");
        }

        public ActionResult RemoveAllFromCart()
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login", "Customers");

            bool success = currentCustomer.RemoveAllFromCart();

            return RedirectToAction("Index", "Products");
        }

        public ActionResult RemoveFromCart(int? ProductID = null)
        {

            if (ProductID == null)
                return RedirectToAction("Index", "Products");

            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login", "Customers");

            // Get the Product
            Product productToRemove = nwEntities.Products.Find(ProductID);
            if (productToRemove == null)
                return RedirectToAction("Index", "Products");

            bool success = currentCustomer.RemoveProductFromCart(productToRemove , 1);

            return RedirectToAction("Index", "Products");

        }

        #endregion


        public ActionResult Analysis(string id)
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null || string.IsNullOrEmpty(id))
                return RedirectToAction("Login");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = nwEntities.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.YearsList = NWTradersUtilities.BeginToEndOrderYears();
            ViewBag.Year = "All Years";
            return View(customer);

        }

        #region Sales

        public ActionResult AverageSales()
        {
            List<OrderRevenues> AnnualAverageSales = new List<OrderRevenues>();

            var AvgOrder = nwEntities.
                Orders.
                Where(o => o.OrderDate.Value.Year < 2021).
                GroupBy(custOrder => new { custOrder.Customer, custOrder.OrderDate.Value.Year }).
                Select(avgOrder => new {
                    C = avgOrder.Key.Customer.CompanyName,
                    Year = avgOrder.Key.Year,
                    avg = avgOrder.Average(o => o.Order_Details.Sum(od => od.Quantity * od.UnitPrice)),
                    annualSpend = avgOrder.Sum(o => o.Order_Details.Sum(od => od.Quantity * od.UnitPrice)),
                    annualNumberOfOrders = avgOrder.Count(),
                    Orders = avgOrder.ToList()
                }).
                ToList();

            var avg = nwEntities.Orders.Where(o => o.OrderDate.Value.Year < 2021).
                Average(o => o.Order_Details.Sum(od => od.Quantity * od.UnitPrice));

            var avgAnnualOrder = nwEntities.Orders.
                                Where(o => o.OrderDate.Value.Year < 2021).
                                GroupBy(o => o.OrderDate.Value.Year).
                                Select(avgOrder => new {
                                    year = avgOrder.Key,
                                    avg = avgOrder.Average(o => o.Order_Details.Sum(od => od.Quantity * od.UnitPrice))
                                }).ToList();


            decimal AverageAnnualCustOrder = AvgOrder.Average(o => o.avg);
            ViewBag.AnnualOrder = AverageAnnualCustOrder;

            decimal AverageAnnualSpend = AvgOrder.Average(o => o.annualSpend);
            ViewBag.AnnualOrder = AverageAnnualCustOrder;

            decimal MaxSpend = AvgOrder.Max(o => o.annualSpend);
            decimal MinSpend = AvgOrder.Min(o => o.annualSpend);


            var AnnualSpend = AvgOrder.
                Select(o => o);

            return View();
        }

        public ActionResult AnnualSales(string CustomerId)
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login");

            Customer theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(currentCustomer.CustomerID)).
                FirstOrDefault();

            IEnumerable<CustomerSales> annualSales = theCustomer.CustomerAnnualSales();

            return PartialView("_AnnualSales", annualSales);
        }

        public JsonResult GetAnnualSales(string CustomerID)
        {
            Customer theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(CustomerID)).
                FirstOrDefault();

            IEnumerable<CustomerSales> annualSales = theCustomer.CustomerAnnualSales();

            return Json(new { JSONList = annualSales }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerSalesModal(string Year = "")
        {
            int year = DateTime.Now.Year;

            if (string.IsNullOrEmpty(Year)) year = int.Parse(Year);
            else year = int.Parse(Year);

            Customer theCustomer = Session["currentCustomer"] as Customer;

            theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(theCustomer.CustomerID)).
                FirstOrDefault();

            IEnumerable<CustomerSales> annualSales = theCustomer.CustomerSalesInYear(Year);

            ViewBag.Year = year;
            return PartialView("_AnnualSalesModal", annualSales);
        }

        public JsonResult GetCustomerSalesModal(string Year = "")
        {
            int year = DateTime.Now.Year;
            if (string.IsNullOrEmpty(Year)) year = int.Parse(Year);
            else year = int.Parse(Year);

            Customer theCustomer = Session["currentCustomer"] as Customer;

            theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(theCustomer.CustomerID)).
                FirstOrDefault();

            IEnumerable<CustomerSales> annualSales = theCustomer.CustomerSalesInYear(Year);
            return Json(new { JSONList = annualSales }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Top Products
        public ActionResult TopProducts(string Year = "All Years")
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login");

            ViewBag.Year = Year;            
            return PartialView("_TopProducts");
        }
        public JsonResult GetTopProducts(string Year = "All Years")
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            Customer theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(currentCustomer.CustomerID)).
                FirstOrDefault();

            ;
            ViewBag.Year = Year;
            return Json(new { JSONList = theCustomer.CustomerSalesInYear(
                Year.Equals("All Years") ? null : Int32.Parse(Year)
                ).OrderByDescending(od => od.Sales).Take(10) }, JsonRequestBehavior.AllowGet);
        }

        #endregion Top Products

        #region Top Product Categories

        public ActionResult TopProductCategories(string Year = "All Years")
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            if (currentCustomer == null)
                return RedirectToAction("Login");

            ViewBag.Year = Year;
            return PartialView("_TopProductCategories");
        }

        public JsonResult GetTopProductCategories(string Year = "All Years")
        {
            Customer currentCustomer = Session["currentCustomer"] as Customer;
            Customer theCustomer = nwEntities.Customers.
                Include(c => c.Orders).
                Where(c => c.CustomerID.Equals(currentCustomer.CustomerID)).
                FirstOrDefault();

            ;
            ViewBag.Year = Year;
            return Json(new
            {
                JSONList = theCustomer.TopProductCategories(
                Year.Equals("All Years") ? null : Int32.Parse(Year)
                ).OrderByDescending(od => od.Sales).Take(10)
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Top Product Categories

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
