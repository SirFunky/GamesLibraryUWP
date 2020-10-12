using GamesLibraryUWP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace GamesLibraryUWP
{
    public sealed partial class MainPage : Page
    {
        private const string BASE_URL = @"http://localhost:32922//api/";
        public MainPage()
        {
            this.InitializeComponent();

        }
        //OnNavigatedTo will execute every time the user goes to the page. The constructor normally only the first time
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadGames();
        }
        public async void LoadGames()
        {
            try
            {
                prgMain.IsActive = true; //Progressbar shown while waiting for webservice to answer
                string URL = BASE_URL + "Games";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var gameList = JsonConvert.DeserializeObject<List<Model.Game>>(content);
                    foreach (var game in gameList)
                    {
                        game.Developer = new List<Developer>();
                        foreach (var gameDeveloper in game.GameDevelopers)
                        {
                            
                            if (gameDeveloper.Developer != null)
                            {
                                game.Developer.Add(gameDeveloper.Developer);
                            }
                            
                        }
                        game.Studio = new List<Studio>();
                        foreach (var gameDeveloper in game.GameDevelopers)
                        {

                            if (gameDeveloper.studio != null)
                            {
                                game.Studio.Add(gameDeveloper.studio);
                            }

                        }

                    }
                        //Databind the list
                        lstGames.ItemsSource = gameList;
                    prgMain.IsActive = false;
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        #region Navigation
        private void MnuAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage));

        }
        #endregion
    }
}

