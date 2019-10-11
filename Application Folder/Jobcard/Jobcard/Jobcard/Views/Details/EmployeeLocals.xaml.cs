using Jobcard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views.Details
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeLocals : ContentPage
    {
        public ListView ListView { get { return listview; } }
        public List<EmployeeListModel> items;
        double lattitude;
        double longitude;

        public EmployeeLocals()
        {
            InitializeComponent();
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

            try
            {
                items = new List<EmployeeListModel>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/employee/AllEmployees/" + Constants.EmpID;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeListModel>>(json);
                ActivitySpinner.IsVisible = false;
                ListView.ItemsSource = items;
            }
            catch (Exception ex)
            { await DisplayAlert("Jobs", "No Jobs On List", "Okay"); }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as EmployeeListModel;

                

                EmployeeLocation empLocal = new EmployeeLocation();


                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/employee/GetLocation/" + item.id;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                empLocal = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeLocation>(json);

                

                if (empLocal.JobDesc == "" || empLocal.JobDesc==null) 
                {
                    await DisplayAlert("Job", "Employee Has No Active Job", "Okay");
                }
                else
                {

                    bool SetActive = await DisplayAlert("Job", empLocal.JobDesc, "View On Map", "No");
                    if (SetActive == true)
                    {
                        string[] arraypoints;
                        string location = empLocal.Coordinates;
                        arraypoints = location.Split(',');

                        string lat = arraypoints[0];
                        lattitude = Convert.ToDouble(lat, CultureInfo.InvariantCulture);
                        string lng = arraypoints[1];
                        longitude = Convert.ToDouble(arraypoints[1], CultureInfo.InvariantCulture);

                        await Map.OpenAsync(lattitude, longitude, new MapLaunchOptions { Name = empLocal.JobDesc });
                    }
                }

            }
            catch(Exception ex)
            {
                await DisplayAlert("Job", "Employee Has No Active Job Catch", "Okay");
            }

            ActivitySpinner.IsVisible = false;
            ListView.ItemsSource = items;

        }
    }
}