using Jobcard.Data;
using Jobcard.Models;
using Jobcard.Views;
using Jobcard.Views.Landing;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Jobcard
{
    public partial class App : Application
    {
        private static Label labelscreen;
        private static bool hasInternet;
        private static Page currentpage;
        private static Timer timer;
        private static bool noInternetShow;

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


        //----------------------Internet Connection
        public static void StartCheckIfInternet(Label label, Page page)
        {
            labelscreen = label;
            label.Text = Constants.NoInternetText;
            label.IsVisible = false;
            hasInternet = true;
            currentpage = page;
            if(timer == null)
            {
                timer = new Timer((e) =>
                {
                    CheckIfInternetOverTime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void CheckIfInternetOverTime()
        {
            var NetworkConnection = DependencyService.Get<INetworkConnection>();
            NetworkConnection.CheckNetworkConnection();
            if(!NetworkConnection.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (hasInternet)
                    {
                        if (!noInternetShow)
                        {
                            hasInternet = false;
                            labelscreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }

                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hasInternet = true;
                    labelscreen.IsVisible = false;
                });
            }

        }

        public static async Task<bool> CheckIfInternet()
        {
            var NetworkConnection = DependencyService.Get<INetworkConnection>();
            NetworkConnection.CheckNetworkConnection();
            return NetworkConnection.IsConnected;
        }
        public static async Task<bool> CheckIfInternetAlert()
        {
            var NetworkConnection = DependencyService.Get<INetworkConnection>();
            NetworkConnection.CheckNetworkConnection();
            if (!NetworkConnection.IsConnected)
            {
                if(!noInternetShow)
                {
                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }

        private static async Task ShowDisplayAlert()
        {
            noInternetShow = false;
            await currentpage.DisplayAlert("Internet", "Device has No Internet, Please Reconnect", "Okay");
            noInternetShow = false;
        }
    }
}
