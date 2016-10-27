using System;
using System.Collections.Generic;

namespace MyMinions.Model
{
    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Reputation { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
