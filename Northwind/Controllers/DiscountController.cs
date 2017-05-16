using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Northwind.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Discount
        public ActionResult Index()
        {
            return View();
        }

        // GET: Discount/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Discount/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Discount discount)
        {
            // Add new customer to database
            using (NorthwndEntities db = new NorthwndEntities())
            {

                int maxDiscountCode = Convert.ToInt32(db.Discounts.Max(d => d.Code));
                discount.Code = maxDiscountCode + 1;

                // save discount to database
                db.Discounts.Add(discount);
                db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
        }
    }
}