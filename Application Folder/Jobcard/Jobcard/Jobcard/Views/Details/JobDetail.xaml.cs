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
using Jobcard.Views.Landing;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobDetail : ContentPage
    {
        double lattitude;
        double longitude;
        int jobid;
        JobDetailModel job = null;
        Constants c = new Constants();

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
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Job", "Successfully Finished Job", "Okay");
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
        }



        async void BtnConfirm_clicked(object sender, EventArgs e)
        {
            switch (pickAction.SelectedItem.ToString())
            {
                case "View On Map":
                    await Map.OpenAsync(lattitude, longitude, new MapLaunchOptions { Name = job.ClientDetails });
                    break;
                case "Complete":
                    completejob();
                    break;
                case "Comment":
                    Comment();
                    break;
                case "Cancel":
                    Cancel();
                    break;
                case "Delete":
                    DeleteJob();
                    break;
                case "Call Client":
                    CallClient();
                    break;
                case null:
                    break;
            }          
        }

        async void CallClient()
        {
            try
            {
                string client = lblClientName.Text;

                string ClientCell = client.Substring(client.IndexOf("-") + 2, 10);

                PhoneDialer.Open(ClientCell);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Job", "Call Error", "Okay");
            }
            
        }


        async void DeleteJob()
        {
            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/job/RemoveJob/" + jobid;
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(job);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Job", "Successfully Deleted Job", "Okay");
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
        }




        async void Cancel()
        {
            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/job/SetUnactive/" + jobid;
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(job);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Job", "Successfully Canceled Job", "Okay");
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
        }






        async void Comment()
        {
            string Comment = await InputBox(this.Navigation,job.Comment);
            
            if (Comment != null && Comment != "" && c.isAllString(Comment)==true)
            {
                HttpClient client = new HttpClient();
                string url = Constants.URL + $"/job/AddComment/" + jobid+"/"+ Comment;
                var uri = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var json = JsonConvert.SerializeObject(job);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    await DisplayAlert("Job", "Successfully Added Comment", "Okay");
                    Application.Current.MainPage = new NavigationPage(new MasterDetail());
                }
                else
                {
                    await DisplayAlert("Job", "Server Communication Error", "Okay");
                }
            }
            else if(Comment == "")
            {
                await DisplayAlert("Job", "Comment should contain 1 or more characters", "Okay");
            }
            else if (Comment == null)
            {
                await DisplayAlert("Job", "Operation Cancelled", "Okay");
            }
            else if (c.isAllString(Comment) != true)
            {
                await DisplayAlert("Job", "Comment should contain only letters A-Z", "Okay");
            }
 
        }

        public static Task<string> InputBox(INavigation navigation,string Comment)
        {

            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Add Comment", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold , TextColor = Constants.MaintextColor };
            var lblMessage = new Label { Text = "Enter new Comment:" , TextColor = Constants.MaintextColor };
            var txtInput = new Entry { Text = Comment};

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,

                BackgroundColor = Constants.MaintextColor,
                TextColor = Color.White,
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,

                BackgroundColor = Constants.MaintextColor,
                TextColor = Color.White,
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }





    }
}