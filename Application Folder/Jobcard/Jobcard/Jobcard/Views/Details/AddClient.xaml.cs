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
        Constants c = new Constants();
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
                if (c.IsCellPhone(edtContact.Text) == true && c.isAllString(edtName.Text) == true && c.isAllString(edtSurname.Text) == true && c.isAllString(edtCompany.Text) == true)
                {
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
                    catch (Exception ex)
                    {
                        await DisplayAlert("Client", "Error Occured", "Okay");
                    }
                }
                else
                {
                    if (c.IsCellPhone(edtContact.Text) == false)
                    { await DisplayAlert("Client", "Input Error - Cell Phone Number not in correct format", "Okay"); }
                    else if (c.isAllString(edtName.Text) == false)
                    { await DisplayAlert("Client", "Input Error - Name field should only contain characters A-Z", "Okay"); }
                    else if (c.isAllString(edtSurname.Text) == false)
                    { await DisplayAlert("Client", "Input Error - Surname field should only contain characters A-Z", "Okay"); }
                    else if (c.isAllString(edtCompany.Text) == false)
                    { await DisplayAlert("Client", "Input Error - Company field should only contain characters A-Z", "Okay"); }
                    else { await DisplayAlert("Client", "Input Error", "Okay"); }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Client", "Input Data Error", "Okay");
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