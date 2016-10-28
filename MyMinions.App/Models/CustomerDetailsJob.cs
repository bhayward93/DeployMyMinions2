using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMinions.App.Models
{
    public class CustomerDetailsJob
    {
        public int Id { get; set; }
        public string Descriptor { get; set; }
        public DateTime StartDate { get; set; }
    }
}