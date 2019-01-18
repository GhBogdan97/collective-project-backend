using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class InternshipManagementViewModel
    {
        public int Id { get; set; }
        public int TotalPlaces {get;set;}
        public int OccupiedPlaces{get;set;}
        public string Name { get; set; }
    }
}
