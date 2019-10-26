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
using Xamarin.Essentials;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddLocation : ContentPage
	{
        Constants c = new Constants();

        public List<Client> items;
		public AddLocation ()
		{
			InitializeComponent ();
            Init();
		}

        async private void BtnAddLocation_Clicked(object sender, EventArgs e)
        {
            if (edtAddress.Text.Length >= 5)
            {
                

                try
                {
                    string clientstring = pickClient.SelectedItem.ToString();
                    int end = clientstring.IndexOf(" ", 0);
                    clientstring = clientstring.Substring(0, end - 0);

                    ActivitySpinner.IsVisible = true;
                    ActivitySpinner.IsRunning = true;
                    Locations location = new Locations();
                    location.Coordinates = edtCoord.Text;
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
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await DisplayAlert("Location", "Successfully Added Location", "Okay");
                        Application.Current.MainPage = new NavigationPage(new MasterDetail());
                    }
                    else
                    {
                        await DisplayAlert("Location", response.ToString(), "Okay");
                    }
                    ActivitySpinner.IsRunning = false;
                    ActivitySpinner.IsVisible = false;



                }
                catch (Exception ex)
                {
                    await DisplayAlert("Job", "No Client Selected", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Job", "Address in wrong format", "Okay");
            }

        }

        async private void Init()
        {

            App.StartCheckIfInternet(lbl_NoInternet, this);
            ActivitySpinner.IsVisible = true;
            ActivitySpinner.IsRunning = true;
            await SetClients();

            lblClient.TextColor = Constants.MaintextColor;
            lblAddress.TextColor = Constants.MaintextColor;
            lblCoordinates.TextColor = Constants.MaintextColor;
            btnAddLocation.BackgroundColor = Constants.MaintextColor;

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);


                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    string lat = location.Latitude.ToString().Replace(",", ".");
                    string lon = location.Longitude.ToString().Replace(",",".");





                    edtCoord.Text = lat + "," + lon;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Location Error", fnsEx.ToString(), "OKAY");
            }
            catch (FeatureNotEnabledException fnsEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Location Error", "Please Turn On Location Services", "OKAY");
            }
            catch (PermissionException fnsEx)
            {
                // Handle permission exception
                await DisplayAlert("Location Error", fnsEx.ToString(), "OKAY");
            }
            catch (Exception fnsEx)
            {
                // Unable to get location
                await DisplayAlert("Location Error","Unable To Get Location", "OKAY");
            }

            ActivitySpinner.IsVisible = false;
            ActivitySpinner.IsRunning = false;

        }






        async Task SetClients()
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
            {
                await DisplayAlert("Client", "No Clients Retrieved", "Okay");
            }
        }

        private async void EdtCoord_Completed(object sender, EventArgs e)
        {
            if(edtCoord.Text.Length > 20)
            {
                try
                {
                    string St = edtCoord.Text;

                    int pFrom = St.IndexOf("/@") + "/@".Length;
                    int pTo = St.LastIndexOf(",");

                    string result = St.Substring(pFrom, pTo - pFrom);

                    edtCoord.Text = result;
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Location", "Location URL Not In Correct Format", "OKAY");
                }
              
            }
        }
    }
}