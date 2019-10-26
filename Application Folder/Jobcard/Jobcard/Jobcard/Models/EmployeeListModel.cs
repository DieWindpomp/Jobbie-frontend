using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class EmployeeListModel
    {
        public int id { get; set; }
        public string EmpName { get; set; }
        public string EmpSurname { get; set; }
        public string EmpContact { get; set; }
        public string EmpPw { get; set; }
        public int Admin { get; set; }
        public bool Exist { get; set; }
    }
}
