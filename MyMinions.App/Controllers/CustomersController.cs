using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyMinions.App.Controllers
{
    public class CustomersController : Controller
    {
        private Model.Context context = new Model.Context();

        public ActionResult Index()
        {
            var customers = context.Customers
                                   .OrderBy(c => c.LastName)
                                   .Select(c => new Models.CustomerIndex
                                   {
                                       Id = c.Id,
                                       FirstName = c.FirstName,
                                       LastName = c.LastName
                                   })
                                   .AsEnumerable();
            return View(customers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
