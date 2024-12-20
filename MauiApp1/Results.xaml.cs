using System.Text.Json;

namespace MauiApp1;

public partial class Results : ContentPage
{
    List<PreviousResult> prev = [];

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

    // Function to display results based on game mode
    private void DisplayResults(string fileName, string title)
    {
        int count = 0;
        gameLayout.Children.Clear();

        foreach (string json in GameFetcher(fileName).Reverse())
        {
            if (!string.IsNullOrEmpty(json))
            {
                prev.Add(JsonSerializer.Deserialize<PreviousResult>(json));
                if (count == 10)
                    break;

                Label label = new Label
                {
                    Text = $"\n\n{title} Results\n\n" +
                          $"Player Name: {prev[count].PlayerName}\n" +
                          $"Questions Answered Correctly: {prev[count].CorrectAnswers}\n" +
                          $" Questions Answered Incorrectly: {prev[count].IncorrectAnswers}\n\n",
                    FontSize = 20,
                    FontFamily = "RubikDirt",
                    HorizontalOptions = LayoutOptions.Center
                };

                // Add additional information specific to the game mode here (if needed)

                VerticalStackLayout horLayout = new VerticalStackLayout
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
                gameLayout.Add(frame);

                count++;
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
        return new string[] { };
    }

    private void TimedSP(object sender, EventArgs e)
    {
        DisplayResults("TimeAttack.json", "Timed Singleplayer");
    }

    private void HotPotato(object sender, EventArgs e)
    {
        DisplayResults("HotPotato.json", "Hot Potato"); // Change filename based on game mode
    }

    private void CoopGame(object sender, EventArgs e)
    {
        DisplayResults("CoopGame.json", "Cooperative"); // Change filename based on game mode
    }

    private void Versus(object sender, EventArgs e)
    {
        DisplayResults("Versus.json", "Versus"); // Change filename based on game mode
    }

    private void SetQuestionsButton(object sender, EventArgs e)
    {
        // Handle setting questions specific to this mode (if needed)
    }
}