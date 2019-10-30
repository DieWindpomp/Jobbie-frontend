using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Jobcard.Models
{
    public class Constants
    {
        public static bool IsDev = true;

        public static Color BackgroundColor = Color.White;
        public static Color MaintextColor = Color.FromRgb(38,133,197);
        public static int LoginIconHeight = 120;

        public static int Admin;

        public static string HelpURL = "http://0027a23f.ngrok.io";

        public static string URL = HelpURL + "/api";


        public static int EmpID;

        public static string NoInternetText = "No Internet Connection, Please Reconnect.";

        public bool isAllString(string text)
        {
            string letterpattern = @"^[a-zA-Z\x20]+$";
            Regex regex = new Regex(letterpattern);
            return regex.IsMatch(text);
        }

        public bool IsCellPhone(string cell)
        {
            string MatchCellPattern =
          @"^(\+\d{0,2})?([\s]?(\(0\))[\s]?)?((\d)+([\s]? | [\-]?)?\d)+\d*$";
            if (cell.Length != 10)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(cell, MatchCellPattern);
            }
       
        }

    }

}
