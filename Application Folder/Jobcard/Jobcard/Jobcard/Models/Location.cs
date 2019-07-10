using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Location
    {
        public string LocationID { get; set; }
        public string Coordinates { get; set; }
        public string Address { get; set; }

        public Location(string LocationID, string Coordinates, string Address)
        {
            this.LocationID = LocationID;
            this.Coordinates = Coordinates;
            this.Address = Address;
        }
    }
}
