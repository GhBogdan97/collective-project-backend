using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class PostViewModel
    {
        public string Date { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Last { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
    }
}
