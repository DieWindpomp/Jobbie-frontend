using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Jobcard.Models
{
    public class MasterMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Color BackgroundColor { get; set; }
        public Type TargetType { get; set; }

        public MasterMenuItem(string title, string iconSource, Color backgroundcolor, Type targettype)
        {
            this.Title = title;
            this.IconSource = iconSource;
            this.BackgroundColor = backgroundcolor;
            this.TargetType = targettype;

        }
    }
}
