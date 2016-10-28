using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyMinions.App.Models
{
    public class CustomerDetails
    {
        public string Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Reputation { get; set; }
        public IEnumerable<CustomerDetailsJob> Jobs { get; set; }
    }
}