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
    public partial class CompletedJobs : ContentPage
    {
        List<JobListItem> items;


        public CompletedJobs()
        {
            InitializeComponent();
            listview.ItemSelected += OnItemSelected;
            Init();
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as JobListItem;

            await DisplayAlert("View Job", item.ToString(), "OKAY");
        }

        async void Init()
        {
            lblEmployee.TextColor = Constants.MaintextColor;
            ActivitySpinner.IsVisible = true;



            App.StartCheckIfInternet(lbl_NoInternet, this);
            await SetEmployees();
            ActivitySpinner.IsVisible = false;
        }

        async Task SetEmployees()
        {
            try
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
            }
            catch (Exception ex)
            {
                await DisplayAlert("Communication Error", "Error Retrieving", "Okay");
            }
            ActivitySpinner.IsVisible = false;
        }
        async Task SetItems(string pickerstring)
        {
            string empstring = pickerstring;
            int end = empstring.IndexOf(" ", 0);
            empstring = empstring.Substring(0, end - 0);

            try
            {
                lblNoItems.IsVisible = false;
                items = new List<JobListItem>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/job/GetJobsByEmpCompleted/" + empstring;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JobListItem>>(json);
                ActivitySpinner.IsVisible = false;
                listview.ItemsSource = items;
                listview.IsVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Jobs", "No Jobs On List", "Okay");
                lblNoItems.IsVisible = true;
                ActivitySpinner.IsVisible = false;
                listview.IsVisible = false;
            }
        }

        private async void PickEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickEmployee.SelectedItem.ToString() != null)
            {
                await SetItems(pickEmployee.SelectedItem.ToString());
            }
            else
            {
                await DisplayAlert("Employee","Please Select An Employee","OKAY");
            }
        }
    }
}