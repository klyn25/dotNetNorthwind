using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Northwind.Controllers.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        [TestMethod()]
        public void ProductTest()
        {
            ProductController pc = new ProductController();
            var result = pc.Product(2) as ViewResult;
            List<Product> p = result.Model as List<Product>;
            Assert.AreEqual(13, p.Count);        }
    }
}