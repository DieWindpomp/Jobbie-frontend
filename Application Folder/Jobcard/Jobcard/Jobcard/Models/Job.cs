using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Job
    {
        public int JobID { get; set; }
        public string JobDescription { get; set; }
        public int LocationID { get; set; }
        public DateTime Date {get;set;}
        public string Urgency { get; set; }
        public bool Active { get; set; }
        public string Comment { get; set; }
        public bool Complete { get; set; }
        public int EmpID { get; set; }
       

        public Job()
        {

        }

    }
}
