using System;

namespace MyMinions.Model
{
    public class Job
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public JobDescriptor Descriptor { get; set; }
        public DateTime StartDate { get; set; }
    }
}
