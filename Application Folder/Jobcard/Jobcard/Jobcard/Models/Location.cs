using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Location
    {
        public string id { get; set; }
        public string Coordinates { get; set; }
        public string Address { get; set; }
        public string ClientID { get; set; }

        public Location()
        {

        }
    }
}
