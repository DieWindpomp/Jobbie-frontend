using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class JobDetailModel
    {
        public int id { get; set; }
        public string ClientDetails { get; set; }
        public string Description { get; set; }
        public int LocationID { get; set; }
        public string Comment { get; set; }
        public string Coordinates { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
    }
}
