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


namespace GamesLibraryUWP
{

    public sealed partial class DeletePage : Page
    {
        private const string BASE_URL = @"http://localhost:32922//api/";
        public DeletePage()
        {
            this.InitializeComponent();
            LoadGamess();
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

        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage));
        }

        private void cmbGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGames.SelectedItem != null)
            {
                Game selectedGame = (Game)cmbGames.SelectedItem;


            }

        }
        public async System.Threading.Tasks.Task DeleteGame()
        {
            try
            {
                HttpClient client = new HttpClient();

                Game deleteGame = (Game)cmbGames.SelectedItem;

                string jsonString = JsonConvert.SerializeObject(deleteGame);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "Games/" + deleteGame.Id;
                var response = await client.DeleteAsync(URL);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
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
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            await DeleteGame();
            var dialog = new MessageDialog("Your game is deleted");
            await dialog.ShowAsync();
            LoadGamess();
        }
    }
}
