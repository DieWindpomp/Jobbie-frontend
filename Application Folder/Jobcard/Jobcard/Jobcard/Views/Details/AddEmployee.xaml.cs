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
	public partial class AddEmployee : ContentPage
	{
		public AddEmployee ()
		{
			InitializeComponent ();
            Init();
		}
        async void AddUserProcedure(object sender, EventArgs e)
        {
            ActivitySpinner.IsEnabled = true;

            AddEmployeeM employee = new AddEmployeeM();
            employee.EmpName = edtName.Text;
            employee.EmpSurname = edtSurname.Text;
            employee.EmpPw = edtPassword.Text;
            employee.EmpContact = edtContact.Text;
            if(switchAdmin.IsToggled == true)
            { employee.Admin = 1; }
            else { employee.Admin = 0; }
            

            HttpClient client = new HttpClient();
            string url = Constants.URL + $"/employee/AddEmployee/";
            var uri = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                await DisplayAlert("Employee", "Successfully Added Employee", "Okay");
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else
            {
                await DisplayAlert("Job", response.ToString(), "Okay");
            }
            ActivitySpinner.IsEnabled = false;
        }
        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            lblName.TextColor = Constants.MaintextColor;
            lblSurname.TextColor = Constants.MaintextColor;
            lblContact.TextColor = Constants.MaintextColor;
            lblCheckAdmin.TextColor = Constants.MaintextColor;
            lblPassword.TextColor = Constants.MaintextColor;
            btnAddUser.BackgroundColor = Constants.MaintextColor;

        }

    }
}