﻿using System.Linq.Expressions;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private Button selectedButton;
        int numQuestions = 0;
        bool firstRun = true;
        private int selectedPlayers = 1;

        public MainPage()
        {
           InitializeComponent();
            if (Preferences.Get("isLightTheme", false))
                logo.Source = ImageSource.FromFile("trivialight.png");
            else
                logo.Source = ImageSource.FromFile("triviadark.png");
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            difficulty.SelectedIndex = 1;
            type.SelectedIndex = 1;
            if (firstRun)
            {
                firstRun = false;
                ChangeTheme(Preferences.Get("isLightTheme", false));
            }
            
            
        }
        private void ChangeTheme(bool isLightTheme)
        {
            if (isLightTheme)
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                logo.Source = ImageSource.FromFile("trivialight.png");
                isLightTheme = true;

            }
            else
            {

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                logo.Source = ImageSource.FromFile("triviadark.png");
                isLightTheme = false;
            }
        }

        private void NumberQuestions(object sender, EventArgs e)
        {
            var button = (Button)sender;
            
            if (selectedButton != null)
            {

                selectedButton.IsEnabled = true;
            }
                
            selectedButton = button;
            if (selectedButton.Text.Equals("5 Questions"))
            {
                numQuestions = 5;
                questionLabel.Text = "You have selected: 5 Questions";
            }
            else if (selectedButton.Text.Equals("10 Questions"))
            {
                numQuestions = 10;
                questionLabel.Text = "You have selected: 10 Questions";
            }
            else if (selectedButton.Text.Equals("15 Questions"))
            {
                numQuestions = 15;
                questionLabel.Text = "You have selected: 15 Questions";
            }
            else
            {
                numQuestions = 20;
                questionLabel.Text = "You have selected: 20 Questions";
            }

            // var primaryColor = (Color)Resources["PrimaryBackgroundColor"];
            selectedButton.IsEnabled = false;
        }

        private async void goButton(object sender, EventArgs e)
        {
            try
            {
                // 1 player game
                if (difficulty.SelectedItem.ToString() != null && numQuestions != 0 && type.SelectedItem.ToString() != null && selectedPlayers == 1)
                {
                    await Navigation.PushAsync(new Game(difficulty.SelectedIndex, numQuestions, type.SelectedIndex));
                }
                else if(difficulty.SelectedItem.ToString() != null && numQuestions != 0 && type.SelectedItem.ToString() != null && selectedPlayers != 1)
                {
                    await Navigation.PushAsync(new MPGame(difficulty.SelectedIndex, numQuestions, type.SelectedIndex, selectedPlayers));
                }
                else
                {
                    DisplayAlert("Invalid Input", "Please enter number of questions", "Return to Selection");
                }
            }
            catch (Exception)
            {
                DisplayAlert("Invalid Input", "Please enter details correctly.", "Return to menu");
                throw;
            }
         
        }


        

        private void Image_Tapped(object sender, TappedEventArgs e)
        {
            // This tap gesture on the pictures allows user to select players
            // Spices it up as opposed to a lot of drop down menus
            selectedPlayers++;

            if (selectedPlayers == 1)
            {
                
                player1.Source = ImageSource.FromFile("selectedplayer.png");
                player2.Source = ImageSource.FromFile("unselectedplayer.png");
                player3.Source = ImageSource.FromFile("unselectedplayer.png");
                player4.Source = ImageSource.FromFile("unselectedplayer.png");
                playersLabel.Text = "1 Player Selected";
            }
            else if (selectedPlayers == 2)
            {
               
                player1.Source = ImageSource.FromFile("selectedplayer.png");
                player2.Source = ImageSource.FromFile("selectedplayer.png");
                playersLabel.Text = "2 Players Selected";
            }
            else if (selectedPlayers == 3)
            {
                
                player1.Source = ImageSource.FromFile("selectedplayer.png");
                player2.Source = ImageSource.FromFile("selectedplayer.png");
                player3.Source = ImageSource.FromFile("selectedplayer.png");
                playersLabel.Text = "3 Player Selected";
            }
            else if (selectedPlayers == 4)
            {
                
                player1.Source = ImageSource.FromFile("selectedplayer.png");
                player2.Source = ImageSource.FromFile("selectedplayer.png");
                player3.Source = ImageSource.FromFile("selectedplayer.png");
                player4.Source = ImageSource.FromFile("selectedplayer.png");
                selectedPlayers = 0; // variable resets to loop
                playersLabel.Text = "4 Player Selected";
            }
        }

       
    }

}
