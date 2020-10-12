using GamesLibraryUWP.Model;
using GamesLibraryUWP.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace GamesLibraryUWP
{
    public sealed partial class AddPage : Page
    {
        private const string BASE_URL = @"http://localhost:32922//api/"; 
        public AddPage()
        {
            this.InitializeComponent();
            LoadDevelopers();
            LoadPublishers();
            LoadStudios();
        }

        private async void LoadPublishers()
        {
            try
            {
                string URL = BASE_URL + "Publishers";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var publisherList = JsonConvert.DeserializeObject<List<Publisher>>(content);

                    cmbPublisher.ItemsSource = publisherList;
                    cmbPublisher.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private async void LoadDevelopers()
        {
            try
            {
                string URL = BASE_URL + "Developers";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var developerList = JsonConvert.DeserializeObject<List<Developer>>(content);

                    //Databind the ViewModel presentation list
                    cmbDeveloper.ItemsSource = GetPresentationList(developerList);
                    cmbDeveloper.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        private async void LoadStudios()
        {
            try
            {
                string URL = BASE_URL + "Studios";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var studioList = JsonConvert.DeserializeObject<List<Developer>>(content);

                    //Databind the ViewModel presentation list
                    cmbDeveloper.ItemsSource = GetPresentationList(studioList);
                    cmbDeveloper.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }



        //In this case we have created a presentation class to show both first and last name. Could be done in simpler ways, but with more complex data it is a good solution
        private List<DeveloperPresentation> GetPresentationList(List<Developer> developerList)
        {
            List<DeveloperPresentation> returnList = new List<DeveloperPresentation>();
            foreach (var developer in developerList)
            {
                DeveloperPresentation presentData = new DeveloperPresentation { Id = developer.Id, Name = developer.Name, Role = developer.Role };
                returnList.Add(presentData);
            }
            return returnList;
        }

        public async System.Threading.Tasks.Task AddGame()
        {
            try
            {
                HttpClient client = new HttpClient();

                //We need to convert back from ViewModel AuthorPresentation to Author
                DeveloperPresentation selectedDeveloper = (DeveloperPresentation)cmbDeveloper.SelectedItem;
                Developer gameDeveloper = new Developer { Id = selectedDeveloper.Id, Name = selectedDeveloper.Name, Role = selectedDeveloper.Role };

                Publisher selectedPublisher = (Publisher)cmbPublisher.SelectedItem;

                Game newGame = new Game();
                newGame.Name = txtName.Text;
                //newGame.PublisherId = selectedPublisher.Id;
                //newGame.GameDevelopers = new List<GameDeveloper>();
                //newGame.GameDevelopers.Add(gameDeveloper);

                string jsonString = JsonConvert.SerializeObject(newGame);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "Games";
                var response = await client.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync(); //ToDo: Handle an error response from web service
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            await AddGame();
            var dialog = new MessageDialog("Your game is saved"); //ToDo: Handle an error response from web service
            await dialog.ShowAsync();
        }

        #region Navigation
        private void navMain_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        private void MnuHome_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        #endregion

    }
}
