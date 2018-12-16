using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Internship
    {
        public Internship()
        {
            Ratings = new List<Rating>();
            Posts = new List<Post>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Places { get; set; }
        public string Topics { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Weeks { get; set; }
        public string Name { get; set; }

        public List<Application> Applications { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Rating> Ratings { get; set; }

        public List<Post> Posts { get; set; }
    }
}
