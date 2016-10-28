using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
                                       LastName = c.LastName,
                                       JobCount = c.Jobs.Count
                                   })
                                   .AsEnumerable();
            return View(customers);
        }

        public async Task<ActionResult> Update()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://myminions.azurewebsites.net/myminions/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.Timeout = TimeSpan.FromMilliseconds(5000);
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/customer");
                response.EnsureSuccessStatusCode();
                var customers =
                    (await response.Content.ReadAsAsync<IEnumerable<Models.CustomerHttp>>())
                                   .Select(c => new Model.Customer
                                   {
                                       Id = c.Id,
                                       FirstName = c.FirstName,
                                       LastName = c.LastName,
                                       Age = c.Age,
                                       Reputation = c.Reputation
                                   })
                                   .ToList();
                customers.ForEach(c =>
                {
                    var customer = context.Customers.Find(c.Id);
                    if (customer == null)
                    {
                        context.Customers.Add(c);
                    }
                    else
                    {
                        customer.Age = c.Age;
                        customer.FirstName = c.FirstName;
                        customer.LastName = c.LastName;
                        customer.Reputation = c.Reputation;
                        context.Entry(customer).State = EntityState.Modified;
                    }
                });
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
            return RedirectToAction("Index");
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
