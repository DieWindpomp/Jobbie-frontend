using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Client
    {
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientContact { get; set; }
        public string CompanyName { get; set; }

        public Client(string ClientID, string ClientName, string ClientSurname, string ClientContact, string CompanyName)
        {
            this.ClientID = ClientID;
            this.ClientName = ClientName;
            this.ClientSurname = ClientSurname;
            this.CompanyName = CompanyName;
        }
    }
}
