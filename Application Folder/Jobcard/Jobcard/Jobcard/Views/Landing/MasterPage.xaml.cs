using Jobcard.Models;
using Jobcard.Views.Details;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Landing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        public ListView ListView { get { return listview; } }
        public List<MasterMenuItem> items;
		public MasterPage ()
		{
			InitializeComponent ();
            Init();
            SetItems();
		}
        void SetItems()
        {
            items = new List<MasterMenuItem>();
            items.Add(new MasterMenuItem("Job List", "document.png", Color.White, typeof(JobList)));
            items.Add(new MasterMenuItem("Current Job Details", "document.png", Color.White, typeof(JobDetail)));
            items.Add(new MasterMenuItem("Add Job", "plus.png", Color.White, typeof(AddJob)));

            
            ListView.ItemsSource = items;
        }
        async void GoToAddJob(object sender, EventArgs e)
        {
            bool x = await DisplayAlert("Logout", "Are you sure you wish to log out?", "Logout", "Cancel");
            if (x == true)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            LogOut.TextColor = Color.White;
            LogOut.BackgroundColor = Color.FromRgb(38, 133, 197);
        }
    }
}