using DatabaseAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Student
    {
        public Student()
        {
            Subscriptions = new List<Subscription>();
            Ratings = new List<Rating>();
        }
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string University { get; set; }
        public string Specialization { get; set; }
        public string College { get; set; }
        public int Year { get; set; }
        public byte[] Cv { get; set; }
        [NotMapped]
        public StudentStatus Status { get; set; }
        
        public List<Subscription> Subscriptions { get; set; }
        public List<Application> Applications { get; set; }

        public List<Rating> Ratings { get; set; }

    }
}
