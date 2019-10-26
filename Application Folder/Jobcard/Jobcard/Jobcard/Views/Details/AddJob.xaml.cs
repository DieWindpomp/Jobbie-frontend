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
	public partial class AddJob : ContentPage
	{
        public List<Client> items;
        public List<Locations> locations;
        public AddJob ()
		{
			InitializeComponent ();
            Init();
		}
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            lblComment.TextColor = Constants.MaintextColor;
            lblDate.TextColor = Constants.MaintextColor;
            lblDescrition.TextColor = Constants.MaintextColor;
            lblUrgency.TextColor = Constants.MaintextColor;
            lblClient.TextColor = Constants.MaintextColor;
            lblLocation.TextColor = Constants.MaintextColor;
            btnAddJob.BackgroundColor = Constants.MaintextColor;
            lblEmployee.TextColor = Constants.MaintextColor;
            


            pickEmployee.IsVisible = false;
            lblEmployee.IsVisible = false;

            SetClients();
            SetEmployees();

            ActivitySpinner.IsRunning = false;
            ActivitySpinner.IsVisible = false;
        }
        async void AddJobProcedure(object sender, EventArgs e)
        {
            if (pickLocation.SelectedItem != null || edtDescription.Text != "" || pickUrgency.SelectedItem != null)
            {
                string locationstring = pickLocation.SelectedItem.ToString();
                int end = locationstring.IndexOf(" ", 0);
                locationstring = locationstring.Substring(0, end - 0);

                ActivitySpinner.IsVisible = true;
                ActivitySpinner.IsRunning = true;

                Job job = new Job();
                job.JobDescription = edtDescription.Text;
                job.LocationID = int.Parse(locationstring);
                job.Urgency = pickUrgency.SelectedItem.ToString();
                job.Date = dpDate.Date;
                job.Comment = edtComment.Text;

                if (Constants.Admin == 1)
                {
                    string employee = pickEmployee.SelectedItem.ToString();
                    end = employee.IndexOf(" ", 0);
                    employee = employee.Substring(0, end - 0);
                    job.EmpID = int.Parse(employee);

                }
                else
                {
                    job.EmpID = Constants.EmpID;
                }
            
            


            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/job/AddJob/";
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(job);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode==System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Job", "Successfully Added Job", "Okay");
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
            ActivitySpinner.IsRunning = false;
            ActivitySpinner.IsVisible = false;
            }
            else
            {
                ActivitySpinner.IsRunning = false;
                ActivitySpinner.IsVisible = false;

                await DisplayAlert("Job", "Not All Fields Entered", "Okay");

            }

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

        void PickLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        async void PickClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            ActivitySpinner.IsRunning = true;
            try
            {
                string clientstring = pickClient.SelectedItem.ToString();
                int end = clientstring.IndexOf(" ", 0);
                clientstring = clientstring.Substring(0, end - 0);


                locations = new List<Locations>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/location/getclientlocations/" + clientstring;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Locations>>(json);
                ActivitySpinner.IsVisible = false;
                var list = new List<string>();
                foreach (Locations c in locations)
                {
                    list.Add(c.id + " " + c.Address);
                }
                pickLocation.ItemsSource = list;
            }
            catch (Exception ex)
            { await DisplayAlert("Client", "No Locations Retrieved", "Okay"); }
            ActivitySpinner.IsRunning = false;
            ActivitySpinner.IsVisible = false;
        }
        async void SetEmployees()
        {

            if (Constants.Admin == 1)
            {
                List<EmployeeListModel> items = new List<EmployeeListModel>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/employee/AllEmployees";
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeListModel>>(json);
                ActivitySpinner.IsVisible = false;
                var list = new List<string>();
                foreach (EmployeeListModel emp in items)
                {
                    list.Add(emp.id + " " + emp.EmpName + " " + emp.EmpSurname);
                }
                pickEmployee.ItemsSource = list;
                pickEmployee.IsVisible = true;
                lblEmployee.IsVisible = true;
            }
        }



        }
}