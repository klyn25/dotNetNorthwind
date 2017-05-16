using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Northwind;

namespace Northwind.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Discount
        [HttpGet]
        public ActionResult Index()
        {
            using (NorthwndEntities db = new NorthwndEntities())
            {
                //stuff goes here...
                return View("Index", db.Discounts.OrderBy(p => p.EndTime).ToList()); ;

                //var query = database.Posts.Join(database.Post_Metas,
                //                post => database.Posts.Where(x => x.ID == id),
                //                meta => database.Post_Metas.Where(x => x.Post_ID == id),
                //                (post, meta) => new { Post = post, Meta = meta });
            }

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