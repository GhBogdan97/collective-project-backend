using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Subscription
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
