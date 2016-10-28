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

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = context.Customers
                                  .Where(c => c.Id == id)
                                  .Include(c => c.Jobs)
                                  .Select(c => new Models.CustomerDetails
                                  {
                                      Id = c.Id,
                                      FirstName = c.FirstName,
                                      LastName = c.LastName,
                                      Age = c.Age,
                                      Reputation = c.Reputation,
                                      Jobs = c.Jobs.Select(j => new Models.CustomerDetailsJob
                                      {
                                          Id = j.Id,
                                          Descriptor = j.Descriptor.Descriptor,
                                          StartDate = j.StartDate
                                      })
                                  })
                                  .FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult AddJob(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.JobDescriptorId = new SelectList(context.JobDescriptors, "Id", "Descriptor");
            var job = new Models.CustomerAddJob
            {
                CustomerId = id
            };
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJob(Models.CustomerAddJob form)
        {
            //if (ModelState.IsValid)
            {
                var customer = context.Customers.Find(form.CustomerId);
                if (customer == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var descriptor = context.JobDescriptors.Find(form.JobDescriptorId);
                if (descriptor == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var job = new Model.Job
                {
                    Customer = customer,
                    Descriptor = descriptor,
                    StartDate = DateTime.Now
                };
                context.Jobs.Add(job);
                context.SaveChanges();
                return RedirectToAction("Details", new { id = form.CustomerId });
            }
            ViewBag.JobDescriptorId = new SelectList(context.JobDescriptors, "Id", "Descriptor");
            return View(form);
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
