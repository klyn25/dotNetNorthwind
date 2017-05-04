using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Northwind.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer/Account
        public ActionResult Account()
        {
            return View();
        }
        // GET: Customer/Register
        public ActionResult Register()
        {
            return View();
        }
        // GET: Customer/SignIn
        public ActionResult SignIn()
        {
            return View();
        }
    }
}