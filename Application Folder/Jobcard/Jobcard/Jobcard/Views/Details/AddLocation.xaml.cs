using Jobcard.Models;
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
	public partial class AddLocation : ContentPage
	{
        public List<Client> items;
		public AddLocation ()
		{
			InitializeComponent ();
            Init();
		}

        async private void BtnAddLocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                string clientstring = pickClient.SelectedItem.ToString();
                int end = clientstring.IndexOf(" ", 0);
                clientstring = clientstring.Substring(0, end - 0);

                ActivitySpinner.IsVisible = true;
                ActivitySpinner.IsRunning = true;
                Location location = new Location();
                location.Coordinates = "1234568487, 1233548";
                location.Address = edtAddress.Text;
                location.ClientID = clientstring;

                //empid = constants
                HttpClient client = new HttpClient();
                string url = Constants.URL + $"/location/addLocation/";
                var uri = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var json = JsonConvert.SerializeObject(location);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    await DisplayAlert("Job", "Successfully Added Job", "Okay");
                }
                else
                {
                    await DisplayAlert("Job", response.ToString(), "Okay");
                }
                ActivitySpinner.IsRunning = false;
                ActivitySpinner.IsVisible = false;
            }
            catch(Exception ex)
            {

            }
        }

        private void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            SetClients();
            lblClient.TextColor = Constants.MaintextColor;
            lblAddress.TextColor = Constants.MaintextColor;
            btnAddLocation.BackgroundColor = Constants.MaintextColor;

        }
        async void SetClients()
        {
            try
            {
                items = new List<Client>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/client/getall";
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Client>>(json);
                ActivitySpinner.IsVisible = false;
                var list = new List<string>();
                foreach (Client c in items)
                {
                    list.Add(c.id + " " + c.Company);
                }
                pickClient.ItemsSource = list;
            }
            catch (Exception ex)
            { await DisplayAlert("Client", "No Clients Retrieved", "Okay"); }
        }
    }
}