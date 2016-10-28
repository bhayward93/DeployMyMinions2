using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyMinions.App.Models
{
    public class CustomerAddJob
    {
        public int Id { get; set; }
        [DisplayName("Customer Id")]
        public string CustomerId { get; set; }
        [DisplayName("Descriptor")]
        public int JobDescriptorId { get; set; }
    }
}