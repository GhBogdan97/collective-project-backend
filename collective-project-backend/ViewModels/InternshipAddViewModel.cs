using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class InternshipAddViewModel
    {
        public string Description { get; set; }
        public int Places { get; set; }
        public string Topics { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Weeks { get; set; }
    }
}
