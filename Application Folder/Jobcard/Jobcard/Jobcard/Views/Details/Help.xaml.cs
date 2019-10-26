using Jobcard.Models;
using Jobcard.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Help : ContentPage
	{
		public Help ()
		{
			InitializeComponent ();
            Init();
		}

        private async void BtnDocument_Clicked(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            ActivitySpinner.IsRunning = true;
               
                await Browser.OpenAsync(Constants.HelpURL + "/downloadFile", BrowserLaunchMode.SystemPreferred);
            Application.Current.MainPage = new NavigationPage(new MasterDetail());
            ActivitySpinner.IsVisible = false;
            ActivitySpinner.IsRunning = false;
        }
        private void Init()
        {
            btnDocument.BackgroundColor = Constants.MaintextColor;

        }
    }
}