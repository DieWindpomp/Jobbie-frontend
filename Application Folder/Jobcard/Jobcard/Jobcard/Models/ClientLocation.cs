using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class ClientLocation
    {
        public string LocationID { get; set; }
        public string ClientID { get; set; }

        public ClientLocation(string locationID, string ClientID)
        {
            this.LocationID = LocationID;
            this.ClientID = ClientID;
        }
    }
}
