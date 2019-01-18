using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels {
	public class CompanyViewModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Description { get; set; }
		public byte[] Logo { get; set; }
		public bool Subscribed { get; set; }
		public List<Internship> Internships { get; set; }
	}
}
