using DatabaseAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Application
    {
        public int InternshipId { get; set; }
        public Internship Internship { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
		
        public ApplicationStatus Status { get; set; }
        
    }
}
