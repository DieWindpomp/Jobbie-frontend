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
    public partial class EditClient : ContentPage
    {
        public List<Client> items;
        public List<Locations> locations;
        Client client = null;
        Locations loc = null;
        Constants c = new Constants();

        public EditClient()
        {
            InitializeComponent();
            Init();
            listview.ItemSelected += OnItemSelected;
            listview2.ItemSelected += OnItemSelected2;

        }

        void Init()
        {
            App.StartCheckIfInternet(lbl_NoInternet, this);
            SetClients();
            listview.IsVisible = true;
            btnback.BackgroundColor = Constants.MaintextColor;
        }
        async void SetClients()
        {
            btnback.IsVisible = false;
            try
            {
                items = new List<Client>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/client/getall";
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Client>>(json);
                
                listview.ItemsSource = items;
                listview.IsVisible = true;
                listview2.IsVisible = false;
                pickAction.IsVisible = true;
                pickAction2.IsVisible = false;
            }
            catch (Exception ex)
            { await DisplayAlert("Client", "No Clients Retrieved", "Okay"); }

            ActivitySpinner.IsVisible = false;
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as Client;
                if (pickAction.SelectedItem.ToString() == "Delete Client")
                {
                    bool q = await DisplayAlert("Delete", "Are you sure you want to delete client " + item.id +" "+ item.Company, "Yes", "Cancel");
                    //Delete Client
                        try
                        {
                            if (q == true)                            
                            {
                                HttpClient client = new HttpClient();
                                string url = Constants.URL + "/client/DeleteClient/" + item.id;
                                var uri = new Uri(url);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                HttpResponseMessage response;
                                var json = JsonConvert.SerializeObject(item);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                response = await client.PostAsync(uri, content);



                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                                {
                                    await DisplayAlert("Client", "Successfully Deleted Client", "Okay");

                                }
                                else
                                {
                                    await DisplayAlert("Job", response.ToString(), "Okay");
                                }
                            }
                        }
                        
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", "Error Deleting Client", "Okay");
                        }
                    SetClients();

                }
                else if (pickAction.SelectedItem.ToString() == "Edit Name")
                {
                    string newname = await InputBox(this.Navigation, item.CName, "Client Name");
                  


                    if (newname != null && newname != "" && c.isAllString(newname) == true)
                    {
                        Client c = new Client();
                        c.id = item.id;
                        c.CName = newname;
                        c.CSurname = item.CSurname;
                        c.Company = item.Company;
                        c.CContact = item.CContact;
                        await UpdateClient(c);
                    }
                    else if (newname == null)
                    {
                        await DisplayAlert("Client", "Operation Cancelled","OKAY");
                        
                    }
                    else if (newname == "")
                    {
                        await DisplayAlert("Client", "Name should Contain More than 1 Character", "OKAY");
                        
                    }
                    else if (c.isAllString(newname) != true)
                    {
                        await DisplayAlert("Client", "Name should Contain Only letters A-Z", "OKAY");
                    }



                    SetClients();
                }
                else if (pickAction.SelectedItem.ToString() == "Edit Surname")
                {
                    string newname = await InputBox(this.Navigation, item.CSurname, "Client Surname");



                    if (newname != null && newname != "" && c.isAllString(newname) == true)
                    {
                        Client c = new Client();
                        c.id = item.id;
                        c.CName = item.CName;
                        c.CSurname = newname;
                        c.Company = item.Company;
                        c.CContact = item.CContact;
                        await UpdateClient(c);
                    }
                    else if (newname == null)
                    {
                        await DisplayAlert("Client", "Operation Cancelled", "OKAY");
         
                    }
                    else if (newname == "")
                    {
                        await DisplayAlert("Client", "Surame should Contain More than 1 Character", "OKAY");
                   
                    }
                    else if (c.isAllString(newname) != true)
                    {
                        await DisplayAlert("Client", "Surname should Contain Only letters A-Z", "OKAY");
               
                    }

                    SetClients();
                }
                else if (pickAction.SelectedItem.ToString() == "Edit Contact Number")
                {
                    string newname = await InputBox(this.Navigation, item.CContact, "Contact Number");



                    if (newname != null && newname != "" && c.IsCellPhone(newname) == true)
                    {
                        Client c = new Client();
                        c.id = item.id;
                        c.CName = item.CName;
                        c.CSurname = item.CSurname;
                        c.Company = item.Company;
                        c.CContact = newname;
                        await UpdateClient(c);
                
                    }
                    else if (newname == null)
                    {
                        await DisplayAlert("Client", "Operation Cancelled", "OKAY");
                  
                    }
                    else if (c.IsCellPhone(newname) != true)
                    {
                        await DisplayAlert("Client", "Contact Number Entered Not Valid", "OKAY");
                  
                    }
                    SetClients();
                }
                else if (pickAction.SelectedItem.ToString() == "Edit Company Name")
                {
                    string newname = await InputBox(this.Navigation, item.Company, "Company Name");



                    if (newname != null && newname != "" && c.isAllString(newname) == true)
                    {
                        Client c = new Client();
                        c.id = item.id;
                        c.CName = item.CName;
                        c.CSurname = item.CSurname;
                        c.Company = newname;
                        c.CContact = item.CContact;
                        await UpdateClient(c);
                       
                    }
                    else if (newname == null)
                    {
                        await DisplayAlert("Client", "Operation Cancelled", "OKAY");
                       
                    }
                    else if (newname == "")
                    {
                        await DisplayAlert("Client", "Company Name should Contain More than 1 Character", "OKAY");
                      
                    }
                    else if (c.isAllString(newname) != true)
                    {
                        await DisplayAlert("Client", "Company Name should Contain Only letters A-Z", "OKAY");
                        
                    }

                    SetClients();
                }
                else if (pickAction.SelectedItem.ToString() == "Edit Locations")
                {
                    SetLocations(item.id);
                }
                else
                {

                }
                
            }
            catch(NullReferenceException ex)
            {
                await DisplayAlert("Error","Please Select A Action","Okay");

                
            }
            
        }
        async void SetLocations(string ClientID)
        {
            btnback.IsVisible = true;
            listview.IsVisible = false;
            listview2.IsVisible = true;
            pickAction.IsVisible = false;
            pickAction2.IsVisible = true;
            try
            {
                locations = new List<Locations>();
                ActivitySpinner.IsVisible = true;
                HttpClient client = new HttpClient();
                string url = Constants.URL + "/location/getclientlocations/" + ClientID;
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Locations>>(json);
                listview2.ItemsSource = locations;

                ActivitySpinner.IsVisible = false;             

            }
            catch (Exception ex)
            { await DisplayAlert("Client", "No Locatons Retrieved", "Okay");
                ActivitySpinner.IsVisible = false;
                SetClients();
            }
        }

        async void OnItemSelected2(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Locations;
            try
            {               
                if (pickAction2.SelectedItem.ToString() == "Edit Coordinates")
                {
                    string newcoordinates = await InputBox(this.Navigation,item.Coordinates,"Coordinates");
                    if (newcoordinates != null && newcoordinates != "")
                    {
                        try
                        {
                            string St = newcoordinates;

                            int pFrom = St.IndexOf("/@") + "/@".Length;
                            int pTo = St.LastIndexOf(",");

                            string result = St.Substring(pFrom, pTo - pFrom);

                            Locations l = new Locations();
                            l.id = item.id;
                            l.ClientID = item.ClientID;
                            l.Address = item.Address;
                            l.Coordinates = newcoordinates;



                            await UpdateLocation(l);
                            SetLocations(item.ClientID);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Location", "Location URL Not In Correct Format", "OKAY");
                        }
                    }
                    else if (newcoordinates == null )
                    {
                        await DisplayAlert("Location", "Operation Cancelled", "OKAY");
                        SetLocations(item.ClientID);
                    }
                    else if (newcoordinates == "")
                    {
                        await DisplayAlert("Location", "Location URL Not In Correct Format", "OKAY");
                        SetLocations(item.ClientID);
                    }
                    


                }
                else if (pickAction2.SelectedItem.ToString() == "Edit Address")
                {
                    string newcoordinates = await InputBox(this.Navigation, item.Address, "Location Address");

                    if (newcoordinates != null && newcoordinates != "")
                    {
                        try
                        {

                            Locations l = new Locations();
                            l.id = item.id;
                            l.ClientID = item.ClientID;
                            l.Address = newcoordinates;
                            l.Coordinates = item.Coordinates;



                            await UpdateLocation(l);
                            SetLocations(item.ClientID);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Location", "Location Address Not In Correct Format", "OKAY");
                        }
                    }
                    else if (newcoordinates == null)
                    {
                        await DisplayAlert("Location", "Operation Cancelled", "OKAY");
                    }
                    else if (newcoordinates == "")
                    {
                        await DisplayAlert("Location", "Location Address Not In Correct Format", "OKAY");
                    }


                }
                else if (pickAction2.SelectedItem.ToString() == "Delete Location")
                {
                    bool q = await DisplayAlert("Delete", "Are you sure you want to delete location " + item.id, "Yes", "Cancel");
                    //Delete Location http
                    try
                    {
                        if (q == true)
                        {
                            HttpClient client = new HttpClient();
                            string url = Constants.URL + $"/location/DeleteLocation/" + item.id;
                            var uri = new Uri(url);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response;
                            var json = JsonConvert.SerializeObject(item);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(uri, content);
                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                await DisplayAlert("Employee", "Successfully Deleted Location", "Okay");

                            }
                            else
                            {
                                await DisplayAlert("Job", response.ToString(), "Okay");
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "Error Deleting Client", "Okay");
                    }




                }
            }
            catch (NullReferenceException ex)
            {
                await DisplayAlert("Error", "Please Select A Action", "Okay");
                
            }
            SetLocations(item.ClientID);
        }

        public static Task<string> InputBox(INavigation navigation, string Comment,string Caption)
        {

            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Edit "+ Caption, HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, TextColor = Constants.MaintextColor };
            var lblMessage = new Label { Text = "Enter new "+ Caption +":", TextColor=Constants.MaintextColor };
            var txtInput = new Entry { Text = Comment };

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
        async Task UpdateClient(Client c)
        {
            ActivitySpinner.IsVisible = true;
           
            try {
                
                        HttpClient client = new HttpClient();
                        string url = Constants.URL + $"/client/UpdateClient";
                        var uri = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response;
                        var json = JsonConvert.SerializeObject(c);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(uri, content);
                        if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            await DisplayAlert("Client", "Successfully Added Client", "Okay");
                            
                        }
                        else
                        {
                            await DisplayAlert("Client", response.ToString(), "Okay");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Client", "Error Occured", "Okay");
                    }
            
            ActivitySpinner.IsVisible = false;
        }
        async Task UpdateLocation(Locations l)
        {
            ActivitySpinner.IsVisible = true;

            try
            {//empid = constants
                HttpClient client = new HttpClient();
                string url = Constants.URL + $"/location/UpdateLocation/";
                var uri = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var json = JsonConvert.SerializeObject(l);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    await DisplayAlert("Job", "Successfully Updated Location", "Okay");
                    
                }
                else
                {
                    await DisplayAlert("Job", response.ToString(), "Okay");
                }
                ActivitySpinner.IsRunning = false;
                ActivitySpinner.IsVisible = false;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Location", "Error Occured", "Okay");
            }
            ActivitySpinner.IsVisible = false;
        }

        private void Btnback_Clicked(object sender, EventArgs e)
        {
            SetClients();
        }
    }
}