using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }

        public Employee() { }

        public Employee(string Username, string Password)
        {
            this.Password = Password;
            this.Username = Username;
        }

        public bool CheckInfo()
        {
            if (!Username.Equals("") && !Password.Equals(""))          
                return true;            
            else            
                return false;
            
        }
    }
}
