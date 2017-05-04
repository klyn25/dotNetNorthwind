using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Northwind.Security;
using System.Web.Security;
using Northwind.Models;
using System.Net;

namespace Northwind.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer/Account
        [Authorize]
        public ActionResult Account()
        {
            if (Request.Cookies["role"].Value != "customer")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customerID = UserAccount.GetUserID();

            using (NorthwndEntities db = new NorthwndEntities())
            {
                Customer customer = db.Customers.Find(customerID);

                CustomerEdit customerEdit = new CustomerEdit
                {//inline array
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    City = customer.City,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country,
                    Phone = customer.Phone,
                    Fax = customer.Fax,
                    Email = customer.Email
                };

                return View(customerEdit);
            }
                
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Account(CustomerEdit customerEdit)
        {
            if (Request.Cookies["role"].Value != "customer")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (NorthwndEntities db = new NorthwndEntities())
            {
                if (ModelState.IsValid)
                {
                    Customer customer = db.Customers.Find(UserAccount.GetUserID());

                    //customer.CompanyName = customerEdit.CompanyName;
                    // if the customer is changing their CompanyName
                    if (customer.CompanyName.ToLower() != customerEdit.CompanyName.ToLower())
                    {
                        // Ensure that the CompanyName is unique
                        if (db.Customers.Any(c => c.CompanyName == customerEdit.CompanyName))
                        {
                            // duplicate CompanyName
                            ModelState.AddModelError("CompanyName", "Duplicate Company Name");
                            return View(customerEdit);
                        }
                        customer.CompanyName = customerEdit.CompanyName;
                    }
                    customer.Address = customerEdit.Address;
                    customer.City = customerEdit.City;
                    customer.ContactName = customerEdit.ContactName;
                    customer.ContactTitle = customerEdit.ContactTitle;
                    customer.Country = customerEdit.Country;
                    customer.Email = customerEdit.Email;
                    customer.Fax = customerEdit.Fax;
                    customer.Phone = customerEdit.Phone;
                    customer.PostalCode = customerEdit.PostalCode;
                    customer.Region = customerEdit.Region;

                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(customerEdit);
        }

        //public CustomerEdit Map(Customer cust)
        //{


        //}

        // GET: Customer/Register
        public ActionResult Register()
        {
            return View();
        }
        // GET: Customer/SignIn
        public ActionResult SignIn()
        {
            using (NorthwndEntities db = new NorthwndEntities())
            {
                // create drop-down list box for company name
                ViewBag.CustomerID = new SelectList(db.Customers.OrderBy(c => c.CompanyName), "CustomerID", "CompanyName").ToList();
            }
            return View();

        }
        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,Password,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            // Add new customer to database
            using (NorthwndEntities db = new NorthwndEntities())
            {
                // first, make sure the CompanyName is unique
                if (db.Customers.Any(c => c.CompanyName == customer.CompanyName))
                {
                    // duplicate CompanyName
                    return View();
                }

                // Generate guid for this customer
                customer.UserGuid = System.Guid.NewGuid();
                // Hash & Salt the customer Password using SHA-1 algorithm
                customer.Password = UserAccount.HashSHA1(customer.Password + customer.UserGuid);
                // save customer to database
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            //return View();
        }
        // POST: Customer/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "CustomerId,Password")] CustomerSignIn customerSignIn, string ReturnUrl)

        {
            

            using (NorthwndEntities db = new NorthwndEntities())
            {
                if (ModelState.IsValid)
                {

                
                    // find customer by CustomerId
                    Customer customer = db.Customers.Find(customerSignIn.CustomerId);
                    // hash & salt the posted password
                    string str = UserAccount.HashSHA1(customerSignIn.Password + customer.UserGuid);
                    // Compared posted Password to customer password
                    if (str == customer.Password)
                    {
                        // Passwords match
                        // authenticate user (this stores the CustomerID in an encrypted cookie)
                        // normally, you would require HTTPS
                        FormsAuthentication.SetAuthCookie(customer.CustomerID.ToString(), false);

                        // send a cookie to the client to indicate that this is a customer
                        HttpCookie myCookie = new HttpCookie("role");
                        myCookie.Value = "customer";
                        Response.Cookies.Add(myCookie);

                        // if there is a return url, redirect to the url
                        if (ReturnUrl != null)
                        {
                            return Redirect(ReturnUrl);
                        }
                        // Redirect to Home page
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }
                    else
                    {
                        // Passwords do not match
                        ModelState.AddModelError("Password", "Please re-enter password");

                    }
                }
                // create drop-down list box for company name
                ViewBag.CustomerID = new SelectList(db.Customers.OrderBy(c => c.CompanyName), "CustomerID", "CompanyName").ToList();
                return View();
            }
        }

    }
}