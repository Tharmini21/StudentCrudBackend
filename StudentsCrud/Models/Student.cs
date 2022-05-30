using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsCrud.Models
{
	public class Student
	{
		public string StudentName { get; set; }
		public string Grade { get; set; }
		public string City { get; set; }
		public string Address { get; set; }
		public string Country { get; set; }
		public string Postal { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public long RowId { get; set; }

	}
}
