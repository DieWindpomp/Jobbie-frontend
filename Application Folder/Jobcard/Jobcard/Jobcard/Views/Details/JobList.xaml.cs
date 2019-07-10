using Jobcard.Data;
using Jobcard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Details
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobList : ContentPage
	{
        public ListView ListView { get { return listview; } }
        public List<JobListItem> items;
        public JobList ()
		{
			InitializeComponent ();
            ActivitySpinner.IsVisible = true;
            Init();
            
            SetItems();
            ActivitySpinner.IsVisible = false;
            listview.ItemSelected += OnItemSelected;
        }
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
        }
        async void SetItems()
        {
            items = new List<JobListItem>();
            ActivitySpinner.IsVisible = true;

            HttpClient client = new HttpClient();
            string url = Constants.URL + "/job/GetJobsByEmp/"+Constants.EmpID;
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            try
            {
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JobListItem>>(json);
                ActivitySpinner.IsVisible = false;
                ListView.ItemsSource = items;
            }
            catch (Exception ex)
            { await DisplayAlert("Jobs", "No Jobs On List", "Okay"); }


        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as JobListItem;
            bool SetActive = await DisplayAlert("Set as active job",item.ToString(),"Yes","No");

        }
    }
}