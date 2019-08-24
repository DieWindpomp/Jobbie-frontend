using Jobcard.Data;
using Jobcard.Models;
using Jobcard.Views.Details;
using Jobcard.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jobcard.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            Init();
		}

        async void LoginProcedure(object sender, EventArgs e)
        {
            ActivitySpinner.IsVisible = true;
            
            Employee employee = new Employee(edtUsername.Text, edtPasword.Text);
            //in ap rest API

            Employee feedback = null;
            HttpClient client = new HttpClient();
            string url = Constants.URL +  "/employee/Login/" + employee.Username +"/"+ employee.Password;
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            try
            {
                feedback = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee>(json);
                if (feedback.id >= 1)
                {
                    await DisplayAlert("Login", "Login Success", "Okay");
                    Constants.EmpID = feedback.id;
                    Application.Current.MainPage = new NavigationPage(new MasterDetail());

                }
                else
                {
                    await DisplayAlert("Login", employee.Username + " is not valid", "Retry");
                }
                
            }
            catch (Exception ex)
            { await DisplayAlert("Login", employee.Username + " is not valid", "Retry"); }
            ActivitySpinner.IsVisible = false;


        }
        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            lblPassword.TextColor = Constants.MaintextColor;
            lblUsername.TextColor = Constants.MaintextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            btnLogin.BackgroundColor = Color.FromRgb(38, 133, 197);

            App.StartCheckIfInternet(lbl_NoInternet, this);

            
            edtUsername.Completed += (s, e) => edtPasword.Focus();
            edtPasword.Completed += (s, e) => LoginProcedure(s,e);
        }

    }
}