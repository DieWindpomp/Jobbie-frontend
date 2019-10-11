using Jobcard.Models;
using Jobcard.Views.Landing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddClient : ContentPage
	{
		public AddClient ()
		{
			InitializeComponent ();
            Init();
		}
        async void AddClientProcedure(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            try
            {
                Client addclient = new Client();
                addclient.CName = edtName.Text;
                addclient.CSurname = edtSurname.Text;
                addclient.CContact = edtContact.Text;
                addclient.Company = edtCompany.Text;

                HttpClient client = new HttpClient();
                string url = Constants.URL + $"/client/addClient";
                var uri = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var json = JsonConvert.SerializeObject(addclient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    await DisplayAlert("Client", "Successfully Added Client", "Okay");
                    Application.Current.MainPage = new NavigationPage(new MasterDetail());
                }
                else
                {
                    await DisplayAlert("Client", response.ToString(), "Okay");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Client", "Error Occured", "Okay");
            }
            ActivitySpinner.IsVisible = false;
        }
        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            lblCompany.TextColor = Constants.MaintextColor;

            lblContact.TextColor = Constants.MaintextColor;

            lblName.TextColor = Constants.MaintextColor;

            lblSurname.TextColor = Constants.MaintextColor;

            App.StartCheckIfInternet(lbl_NoInternet, this);
            ActivitySpinner.IsVisible = false;
            btnAddUser.BackgroundColor = Color.FromRgb(38, 133, 197);
        }

    }
}