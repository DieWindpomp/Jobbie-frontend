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

        EmployeeListModel emp = null;

        Constants c = new Constants();
		public AddEmployee ()
		{
			InitializeComponent ();
            Init();
		}
        async void AddUserProcedure(object sender, EventArgs e)
        {

            if (c.isAllString(edtName.Text) == true || c.isAllString(edtSurname.Text) == true || c.IsCellPhone(edtContact.Text) == true)
            {
                ActivitySpinner.IsEnabled = true;

                AddEmployeeM employee = new AddEmployeeM();
                
                employee.EmpName = edtName.Text;
                employee.EmpSurname = edtSurname.Text;
                employee.EmpPw = edtPassword.Text;
                employee.EmpContact = edtContact.Text;
                if (switchAdmin.IsToggled == true)
                { employee.Admin = 1; }
                else { employee.Admin = 0; }

                if (switchExist.IsToggled == true && Constants.Admin == 1)
                {
                    string empstring = pickEmployee.SelectedItem.ToString();
                    int end = empstring.IndexOf(" ", 0);
                    empstring = empstring.Substring(0, end - 0);
                    employee.id = int.Parse(empstring);

                    

                    HttpClient client = new HttpClient();
                    string url = Constants.URL + $"/employee/UpdateEmployee/";
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    var json = JsonConvert.SerializeObject(employee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(uri, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await DisplayAlert("Employee", "Successfully Updated Employee", "Okay");
                        Application.Current.MainPage = new NavigationPage(new MasterDetail());
                    }
                    else
                    {
                        await DisplayAlert("Job", response.ToString(), "Okay");
                    }
                    ActivitySpinner.IsEnabled = false;
                }
                else if (switchExist.IsToggled == false && Constants.Admin == 0)
                {
                    employee.id = Constants.EmpID;
                    HttpClient client = new HttpClient();
                    string url = Constants.URL + $"/employee/UpdateEmployee/";
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    var json = JsonConvert.SerializeObject(employee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(uri, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await DisplayAlert("Employee", "Successfully Updated Employee", "Okay");
                        Application.Current.MainPage = new NavigationPage(new MasterDetail());
                    }
                    else
                    {
                        await DisplayAlert("Employee", response.ToString(), "Okay");
                    }
                    ActivitySpinner.IsEnabled = false;
                }
                else if (switchExist.IsToggled == false && Constants.Admin == 1)
                {
                    employee.id = Constants.EmpID;
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
                        await DisplayAlert("Employee", response.ToString(), "Okay");
                    }
                    ActivitySpinner.IsEnabled = false;
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
                else if (edtPassword.Text == "")
                { await DisplayAlert("Client", "Input Error - Password field should Not Be empty", "Okay"); }
                else { await DisplayAlert("Client", "Input Error", "Okay"); }
            }




        }
        async void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            lblName.TextColor = Constants.MaintextColor;
            lblSurname.TextColor = Constants.MaintextColor;
            lblContact.TextColor = Constants.MaintextColor;
            lblCheckAdmin.TextColor = Constants.MaintextColor;
            lblPassword.TextColor = Constants.MaintextColor;
            btnAddUser.BackgroundColor = Constants.MaintextColor;
            lblEmployee.TextColor = Constants.MaintextColor;
            lblEditExisting.TextColor = Constants.MaintextColor;
            btnDeleteSelected.BackgroundColor = Constants.MaintextColor;
            lblCheckAdmin.IsVisible = true;
            switchAdmin.IsVisible = true;

            if (Constants.Admin != 1)
            {
                await SetEdits();
                btnAddUser.Text = "UPDATE";
            }
            else
            {
                grid1.IsVisible = true;
                
            }

        }

        async Task SetEdits()
        {
            try
            {
                string empstring;

                if (Constants.Admin == 1)
                {
                    empstring = pickEmployee.SelectedItem.ToString();
                    int end = empstring.IndexOf(" ", 0);
                    empstring = empstring.Substring(0, end - 0);
                }
                else
                {
                    empstring = (Constants.EmpID).ToString();
                }

                EmployeeListModel item = new EmployeeListModel();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/employee/GetEmp/" + empstring;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                item = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeListModel>(json);
                emp = item;
                edtName.Text = item.EmpName;
                edtSurname.Text = item.EmpSurname;
                edtPassword.Text = item.EmpPw;
                edtContact.Text = item.EmpContact;
                if (item.Admin == 1)
                {
                    switchAdmin.IsToggled = true;
                }

                ActivitySpinner.IsVisible = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Communication Error", "Error Retrieving", "Okay");
            }
        }

        async void IndexChanged(object sender, EventArgs e)
        {
            await SetEdits();

        }

        async Task SetEmployees()
        {

            if (Constants.Admin == 1)
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
                    pickEmployee.IsVisible = true;
                    lblEmployee.IsVisible = true;
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Communication Error", "Error Retrieving", "Okay");
                }
            }
            else
            {

            }
        }
        


        async void OnToggle(object sender, EventArgs e)
        {
            if (switchExist.IsToggled == true)
            {
                await SetEmployees();
                btnDeleteSelected.IsVisible = true;
                btnAddUser.Text = "UPDATE USER";
                grid2.IsVisible = true;
            }
            else
            {
                pickEmployee.IsVisible = false;
                lblEmployee.IsVisible = false;
                btnDeleteSelected.IsVisible = false;
                btnAddUser.Text = "ADD USER";
                grid2.IsVisible = false;
            }
        }

        async void BtnDeleteSelected_Clicked(object sender, EventArgs e)
        {

            bool rusure = await DisplayAlert("Delete", "Are you sure you want to Delete user: " + pickEmployee.SelectedItem.ToString(), "Yes", "Cancel");
            ActivitySpinner.IsRunning = true;
            ActivitySpinner.IsVisible = true;

            try
            {
                if (rusure == true)
                {
                    string empstring = pickEmployee.SelectedItem.ToString();
                    int end = empstring.IndexOf(" ", 0);
                    empstring = empstring.Substring(0, end - 0);

                    EmployeeListModel emp = new EmployeeListModel();
                    emp.id = int.Parse(empstring);


                    HttpClient client = new HttpClient();
                    string url = Constants.URL + $"/employee/DeleteEmployee/" + empstring;
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    var json = JsonConvert.SerializeObject(emp);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(uri, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        await DisplayAlert("Employee", "Successfully Deleted Employee", "Okay");

                    }
                    else
                    {
                        await DisplayAlert("Job", response.ToString(), "Okay");
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "Error Deleting Employee", "Okay");
            }
                

                ActivitySpinner.IsRunning = false;
                ActivitySpinner.IsVisible = false;
            }
        }
    }
