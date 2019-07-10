using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using Jobcard.Data;
using Jobcard.iOS.Data;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_IOS))]

namespace Jobcard.iOS.Data
{
    public class SQLite_IOS : ISQLite
    {
        public SQLite_IOS() { }

        public SQLite.SQLiteConnection GetConnection()
        {
            var fileName = "Jobcard.db3";
            string documentpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var librarypath = Path.Combine(documentpath,"..", "Library");
            var path = Path.Combine(librarypath, fileName);
            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }
    }
}