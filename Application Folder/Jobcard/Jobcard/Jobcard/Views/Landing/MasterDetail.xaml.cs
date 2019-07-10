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
	public partial class MasterDetail : MasterDetailPage
	{
        bool logout;
		public MasterDetail ()
		{
			InitializeComponent ();
            masterpage.ListView.ItemSelected += OnItemSelected;
		}
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItem;

            if (item != null)
            {
                if (item.TargetType == typeof(LoginPage))
                {
                    GetLogoutAnswer();
                    if (logout == true)
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        masterpage.ListView.SelectedItem = null;
                        IsPresented = false;
                    }
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterpage.ListView.SelectedItem = null;
                    IsPresented = false;
                }
            }
        }
        async void GetLogoutAnswer()
        {
            bool x = await DisplayAlert("Logout", "Are you sure you wish to log out?", "Logout", "Cancel");
            logout = x;
        }
	}
}