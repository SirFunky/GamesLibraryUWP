using GamesLibraryUWP.Model;
using GamesLibraryUWP.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GamesLibraryUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdatePage : Page
    {
        private const string BASE_URL = @"http://localhost:32922//api/";
        public UpdatePage()
        {
            this.InitializeComponent();
            LoadDevelopers();
            LoadPublishers();
            LoadStudios();
            LoadGamess();
        }
        private async void LoadGamess()
        {
            try
            {
                string URL = BASE_URL + "Games";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var gameList = JsonConvert.DeserializeObject<List<Game>>(content);

                    cmbGames.ItemsSource = gameList;
                    cmbGames.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
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

                    cmbDeveloper.ItemsSource = GetPresentationList(developerList);
                    cmbDeveloper.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
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

                    cmbStudio.ItemsSource = GetPresentationList(studioList);
                    cmbStudio.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

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
        private void MnuAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage));

        }

        private void mnuUpdate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdatePage));
        }
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
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //await AddGame();
            var dialog = new MessageDialog("Your game is saved");
            await dialog.ShowAsync();
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage));
        }

        private void cmbGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbGames.SelectedItem != null)
            {
                Game selectedGame = (Game)cmbGames.SelectedItem;
                txtGener.Text = selectedGame.Gener;
                //txtNumPlayers = selectedGame.NumberOfPlayers;
                string developers = "";
                foreach (var developer in selectedGame.Developer)
                {
                    developers += developer.Name + " ";
                }
                string studios = "";
                foreach (var studio in selectedGame.Studio)
                {
                    studios += studio.Name + " ";
                }
                string publisher = "";

               
                cmbDeveloper.Text = developers;
                cmbStudio.Text = studios;
                Publisher selectedPublisher = (Publisher)cmbPublisher.SelectedItem;

            }
            
        }
        public async System.Threading.Tasks.Task UppdateGame()
        {
            try
            {
                HttpClient client = new HttpClient();

                DeveloperPresentation selectedDeveloper = (DeveloperPresentation)cmbDeveloper.SelectedItem;
                Developer gameDeveloper = new Developer { Id = selectedDeveloper.Id, Name = selectedDeveloper.Name, Role = selectedDeveloper.Role };

                Publisher selectedPublisher = (Publisher)cmbPublisher.SelectedItem;

                StudioPresentation selectedStudio = (StudioPresentation)cmbStudio.SelectedItem;
                Studio gameStudio = new Studio { Id = selectedStudio.Id, Name = selectedStudio.Name };

                Game updatedGame = (Game)cmbGames.SelectedItem;
                updatedGame.Name = txtName.Text;
                updatedGame.Gener = txtGener.Text;
                updatedGame.NumberOfPlayers = int.Parse(txtNumPlayers.Text);
                updatedGame.Publisher.Id = selectedPublisher.Id;
                updatedGame.Studio = new List<Studio>();
                updatedGame.Studio.Add(gameStudio);
                updatedGame.Developer = new List<Developer>();
                updatedGame.Developer.Add(gameDeveloper);

                string jsonString = JsonConvert.SerializeObject(updatedGame);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "Games/" + updatedGame.Id;
                var response = await client.PutAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
