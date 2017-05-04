using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Northwind;

namespace Northwind.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/Category
        public ActionResult Category()
        {
            //try
            //{ 
            //    NorthwndEntities db = new NorthwndEntities();
            //    stuff goes here....
            //}
            //finally
            //{
            //    db.Dispose();//dispose of db connection
            //} ...or

            using (NorthwndEntities db = new NorthwndEntities())
            {
                //stuff goes here...
                return View(db.Categories.ToList());
            }

                
        }

        // POST: Product/SearchResults
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResults(FormCollection Form)
        {
            string SearchString = Form["SearchString"];
            ViewBag.Filter = "Product";
            using (NorthwndEntities db = new NorthwndEntities())
            {
                return View("Product", db.Products.Where(p => p.ProductName.Contains(SearchString) && p.Discontinued == false).OrderBy(p => p.ProductName).ToList());
            }
        }

        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (NorthwndEntities db = new NorthwndEntities())
            {
                ViewBag.Filter = db.Categories.Find(id).CategoryName;
                return View(db.Products.Where(p => p.CategoryID == id).ToList());
            }
            
        }

        // GET: Product/Discount
        public ActionResult Discount()
        {
            return View();
        }

        // GET: Product/FilterProducts
        public JsonResult FilterProducts(int? id, string SearchString, decimal? PriceFilter)
        //public JsonResult FilterProducts(decimal? PriceFilter)
        {
            ViewBag.id = id;
            ViewBag.SearchString = SearchString;
            using (NorthwndEntities db = new NorthwndEntities())
            {
                // if there is no PriceFilter, return Http Bad Request
                if (PriceFilter == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                }
               
                var Products = db.Products.Where(p => p.Discontinued == false);

                if (id != null)
                {
                    Products = Products.Where(p => p.CategoryID == id);
                }
                if (!String.IsNullOrEmpty(SearchString))
                {
                    Products = Products.Where(p => p.ProductName.Contains(SearchString));
                }

                var ProductDTOs = (from p in Products.Where(p => p.UnitPrice >= PriceFilter)
                                   orderby p.ProductName
                                   select new
                                   {
                                       p.ProductID,
                                       p.ProductName,
                                       p.QuantityPerUnit,
                                       p.UnitPrice,
                                       p.UnitsInStock
                                   }).ToList();

                return Json(ProductDTOs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}