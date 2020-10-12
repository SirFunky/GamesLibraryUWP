﻿using GamesLibraryUWP.Model;
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
                    var gameList = JsonConvert.DeserializeObject<List<Game>>(content);

                    foreach (var game in gameList)
                    {
                        string developer = "";
                        foreach (var gameDeveloper in game.GameDevelopers)
                        {
                            game.Developer = gameDeveloper.Developer;
                        }
                        //string studio = "";
                        //foreach (var gamedeveloper in game.GameDevelopers)
                        //{
                        //    studio += gamedeveloper.Studio.Name + " ";
                        //}
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

