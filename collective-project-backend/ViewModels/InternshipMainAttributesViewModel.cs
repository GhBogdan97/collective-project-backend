using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class InternshipMainAttributesViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Places { get; set; }
        public string Topics { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Weeks { get; set; }
        public string Name { get; set; }
    }
}
