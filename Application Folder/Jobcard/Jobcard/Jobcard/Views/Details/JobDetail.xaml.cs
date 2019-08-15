using Jobcard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobDetail : ContentPage
    {

        public JobDetail ()
		{
			InitializeComponent ();
            Init();
		}
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            GetJob(Constants.EmpID);
            btnConfirm.Source = "checked.png";
            btnBack.Source = "back.png";
            

        }
        async void GetJob(int EmpID)
        {
            ActivitySpinner.IsVisible = true;
            JobDetailModel job = null;
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
                ActivitySpinner.IsVisible = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Jobs", "No Active Job", "Okay");                
            }


        }
    }
}