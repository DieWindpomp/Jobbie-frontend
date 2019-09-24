using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Jobcard.Models
{
    public class Constants
    {
        public static bool IsDev = true;

        public static Color BackgroundColor = Color.White;
        public static Color MaintextColor = Color.FromRgb(38,133,197);
        public static int LoginIconHeight = 120;

        public static string URL = "http://255e1cf2.ngrok.io/api";

        public static int EmpID;

        public static string NoInternetText = "No Internet Connection, Please Reconnect.";
    }

}
