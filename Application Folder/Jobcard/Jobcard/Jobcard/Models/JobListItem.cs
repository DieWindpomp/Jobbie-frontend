using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class JobListItem
    {
        public string id { get; set; }
        public string Description { get; set; }
        public string Urgency { get; set; }
        public string Address { get; set; }
        public string ClientDetails { get; set; }
        public string CompanyName { get; set; }

        public override string ToString()
        {
            string result = id + " " + Description + "\nfor " + CompanyName + "\nat " + Address + "\nContact Person: "+ClientDetails;
            return result;
        }
    }
}
