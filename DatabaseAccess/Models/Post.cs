using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseAccess.Models
{
    public class Post
    {        public int Id { get; set; }        public DateTime Date { get; set; }        [Required]        public string Title { get; set; }        public bool Last { get; set; }        public byte[] Image { get; set; }        public int InternshipId { get; set; }        public Internship Internship { get; set; }
    }
}
