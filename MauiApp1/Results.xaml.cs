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
}