using System.Linq.Expressions;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private Button selectedButton;
        int numQuestions = 0;
        bool firstRun = true;
        
        public MainPage()
        {
           InitializeComponent();
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
                isLightTheme = true;

            }
            else
            {

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
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
                if (difficulty.SelectedItem.ToString() != null && numQuestions != 0 && type.SelectedItem.ToString() != null)
                {
                    await Navigation.PushAsync(new Game(difficulty.SelectedIndex, numQuestions, type.SelectedIndex));
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
    }

}
