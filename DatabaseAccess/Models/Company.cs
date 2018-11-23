using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Company
    {
        public Company()
        {
            Subscriptions = new List<Subscription>();
            Internships = new List<Internship>();
        }

        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public byte[] Logo { get; set; }

        public List<Subscription> Subscriptions { get; set; }
        public List<Internship> Internships { get; set; }
    }
}
