using System.Text.Json;

namespace MauiApp1;

public partial class Results : ContentPage
{

    List<PreviousResult> prev = [];
    // I decided to avoid using viewmodels as the different gamemodes all have such different parameters that it would have been more effor than it's worth
	public Results()
	{
		InitializeComponent();
        
	}
    private void SingleplayerButtonClicked(object sender, EventArgs e)
    {
        SingleplayerModes.IsVisible = true;
        MultiplayerModes.IsVisible = false;
    }

    private void MultiplayerButtonClicked(object sender, EventArgs e)
    {
        SingleplayerModes.IsVisible = false;
        MultiplayerModes.IsVisible = true;
    }


    // SinglePlayer Set Questions
    private void SetQuestionsButton(object sender, EventArgs e)
    {
        
        int count = 0;
        gameLayout.Children.Clear();
        // Iterates over each line and deserialize it (function is what retrieves all games for that file.)
        // Reverse so it shows newest first and only shows last 10 games, adding dates and searching would be possible however it is outside of my scope.
        foreach (string json in GameFetcher("SPSet.json").Reverse())
        {
            if (!string.IsNullOrEmpty(json))
            {
                prev.Add(JsonSerializer.Deserialize<PreviousResult>(json));
                Label label = new Label { 
                
                    Text = "Player Name: " + prev[count].PlayerName + "\nQuestions Answered Correctly: " + prev[count].CorrectAnswers + "\n Questions Answered Incorrectly: " + prev[count++].IncorrectAnswers + "\n\n",
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colors.CadetBlue,
                    FontSize = 22,
                    FontFamily = "ApeMount"
                    
                };

                gameLayout.Add(label);

            }
        }

    }

    // Function Below returns an array of all of the games
    private static string[] GameFetcher(string fileName)
    {
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
        if (File.Exists(targetFile))
        {
            string jsonString = File.ReadAllText(targetFile);

            string[] jsonObjects = jsonString.Split("\n");

            return jsonObjects;
        }
        return [];
        
    }

    private void TimedSP(object sender, EventArgs e)
    {
        int count = 0;
        gameLayout.Children.Clear();
        // Iterates over each line and deserialize it (function is what retrieves all games for that file.)
        // Reverse so it shows newest first and only shows last 10 games, adding dates and searching would be possible however it is outside of my scope.
        foreach (string json in GameFetcher("TimeAttack.json").Reverse())
        {
            if (!string.IsNullOrEmpty(json))
            {
                prev.Add(JsonSerializer.Deserialize<PreviousResult>(json));
                if (count == 10)
                    break;
                Label label = new Label
                {
                    Text = $"\n\n{title} Results\n\n" +
                          $"Total Questions Answered Correctly: {prev[count].CorrectAnswers}\n" +
                          $"Total Questions Answered Incorrectly: {prev[count].IncorrectAnswers}\n",
                    FontSize = 20,
                    FontFamily = "RubikDirt"
                };

                if (title.Contains("Timed") || title.Contains("Potato"))
                {
                    label.Text += $"Timer Length: {prev[count].TimerLength}\n";
                }

                VerticalStackLayout verticalLayout = new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center

                };
                Frame frame = new Frame
                {
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.MidnightBlue,
                    WidthRequest = 350,
                    CornerRadius = 20,
                    Content = horLayout,
                    Margin = 10,

                };
                Button button = new Button
                {
                    Text = "View Game",
                    HeightRequest = 60,
                    Margin = 6,
                    HorizontalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.DarkSlateBlue,
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center,
                    FontFamily = "RubikDirt"
                };
                
                horLayout.Children.Add(label);
                horLayout.Children.Add(button);
                frame.Content = horLayout;
                //horLayout.Children.Add(frame);

                //Content = gameLayout;
                gameLayout.Add(frame);

            }
        }
    }

    private void HotPotato(object sender, EventArgs e)
    {
        int count = 0;
        gameLayout.Children.Clear();
        // Iterates over each line and deserialize it (function is what retrieves all games for that file.)
        // Reverse so it shows newest first and only shows last 10 games, adding dates and searching would be possible however it is outside of my scope.
        foreach (string json in GameFetcher("TimeAttack.json").Reverse())
        {
            if (!string.IsNullOrEmpty(json))
            {
                prev.Add(JsonSerializer.Deserialize<PreviousResult>(json));
                if (count == 10)
                    break;
                Label label = new Label
                {

                    Text = "Player Name: "
                    + prev[count].PlayerName
                    + "\nQuestions Answered Correctly: "
                    + prev[count].CorrectAnswers
                    + "\n Questions Answered Incorrectly: "
                    + prev[count].IncorrectAnswers
                    + "\nTimer Length: "
                    + prev[count++].TimerLength
                    + "\n\n",
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colors.CadetBlue,
                    FontSize = 22,
                    FontFamily = "ApeMount"

                };

                gameLayout.Add(label);

            }
        }
    }



    private void CoopGame(object sender, EventArgs e)
    {

    }

    private void Versus(object sender, EventArgs e)
    {
        DisplayResults("Versus.json", "Versus"); // Multiplayer versus (everyone answers their own questions)
    }

}