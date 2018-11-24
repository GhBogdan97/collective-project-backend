using DatabaseAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
	public class ApplicationViewModel
	{
		public int InternshipId { get; set; }
		public int StudentId { get; set; }
		public bool Accepted { get; set; }
		public ApplicationStatus Status { get; set; }
	}
}
