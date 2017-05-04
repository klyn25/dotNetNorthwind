using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Northwind.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/Category
        public ActionResult Category()
        {
            return View();
        }
        // GET: Product/Discount
        public ActionResult Discount()
        {
            return View();
        }
    }
}