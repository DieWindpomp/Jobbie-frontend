using Jobcard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobDetail : ContentPage
    {
        double lattitude;
        double longitude;
        int jobid;
        JobDetailModel job = null;

        public JobDetail ()
		{
			InitializeComponent ();
            Init();
		}
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            GetJob(Constants.EmpID);
            lblJobLocation.TextColor= Constants.MaintextColor;
            lblJobDescription.TextColor = Constants.MaintextColor;
            lblClientName.TextColor = Constants.MaintextColor;
            btnConfirm.Source = "checked.png";
            btnBack.Source = "back.png";
            pickAction.SelectedItem = 4;
            

        }
        async void GetJob(int EmpID)
        {
            ActivitySpinner.IsVisible = true;
            
            HttpClient client = new HttpClient();
            string url = Constants.URL + "/job/GetActiveJobByEmp/" + Constants.EmpID;
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            try
            {
                job = Newtonsoft.Json.JsonConvert.DeserializeObject<JobDetailModel>(json);
                lblJobHeader.Text = job.CompanyName;
                lblJobDescription.Text = job.Description;
                lblJobLocation.Text = job.Address;
                lblClientName.Text = job.ClientDetails;
                lblComment.Text = job.Comment;
                jobid = job.id;
                lblViewOnMaps.Text = job.Coordinates;
                ActivitySpinner.IsVisible = false; 

                string[] arraypoints;
                string location = job.Coordinates;
                arraypoints = location.Split(',');

                string lat = arraypoints[0];
                lattitude = Convert.ToDouble(lat, CultureInfo.InvariantCulture);
                string lng = arraypoints[1];
                longitude = Convert.ToDouble(arraypoints[1], CultureInfo.InvariantCulture);








            }
            catch (Exception ex)
            {
                await DisplayAlert("Jobs", "No Active Job", "Okay");
                btnConfirm.IsEnabled = false;
            }




        }
        async void completejob()
        {

            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/job/completeJob/"+jobid;
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(job);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                await DisplayAlert("Job", "Successfully Finished Job", "Okay");
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
        }



        async void btnConfirm_clicked(object sender, EventArgs e)
        {
           switch(pickAction.SelectedItem.ToString())
            {
                case "View On Map":
                    await Map.OpenAsync(lattitude, longitude, new MapLaunchOptions { Name = job.ClientDetails });
                    break;
                case "Complete":
                    completejob();
                    break;
                case null:
                    break;
            }
            
        }


    }
}