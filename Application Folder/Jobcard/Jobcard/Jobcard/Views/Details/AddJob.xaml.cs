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
	public partial class AddJob : ContentPage
	{
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
            lblLocationID.TextColor = Constants.MaintextColor;
            lblUrgency.TextColor = Constants.MaintextColor;

            btnAddJob.BackgroundColor = Constants.MaintextColor;

            ActivitySpinner.IsRunning = false;
            ActivitySpinner.IsVisible = false;
        }
        async void AddJobProcedure(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            ActivitySpinner.IsRunning = true;
            Job job = new Job();
            job.JobDescription = edtDescription.Text;
            job.LocationID = int.Parse(edtLocation.Text);
            job.Urgency = pickUrgency.SelectedItem.ToString();
            job.Date = dpDate.Date;
            job.Comment = edtComment.Text;
            job.EmpID = Constants.EmpID;
            //empid = constants
            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/job/AddJob/";
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(job);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode==System.Net.HttpStatusCode.Accepted)
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
    }
}