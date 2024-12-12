using System.Security.AccessControl;
using System.Text.Json;

namespace MauiApp1;

public partial class ResultsPage : ContentPage
{
	public ResultsPage()
	{
		InitializeComponent();
	}

    // MP Timed Game below (Hot potato)
    
    public ResultsPage(int timerLength, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList)
    {
        InitializeComponent();
    }
    // Hot potato ran out
    //await Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), names[currentPlayer], QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType, names, ranOut));
    public ResultsPage(int timerLength, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList, bool ranOut)
    {
        InitializeComponent();
    }

    // Singleplayer Timed (Time AttacK)
    public ResultsPage(int timer, string playerName, int questionsCorrect, int questionsIncorrect, string Difficulty, string questionType)
    {
        InitializeComponent();
        questionsCorrectLabel.Text += questionsIncorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
    }
    // Singleplayer Timed (hit 50 questions (win))
    public ResultsPage(int timer, string playerName, int questionsCorrect, int questionsIncorrect, string Difficulty, string questionType, bool ranOut)
    {
        InitializeComponent();
        questionsCorrectLabel.Text += questionsIncorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
    }

    // MP Versus
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty , string numberOfQuestions, string questionType)
    {
        InitializeComponent();
        questionsCorrectLabel.Text += questionsCorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
    }

 
    // sp Set questions 
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty, string numberOfQuestions, string questionType, int numberOfPlayers)
    {
        InitializeComponent();
        questionsCorrectLabel.Text += questionsCorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
    }
    // Gamemode 5 (co-op)
    public ResultsPage(int questionsCorrect, int questionsIncorrect, List<string> names, int gameMode)
    {
        InitializeComponent();
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Players = names,
            GameMode = gameMode
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        try
        {
            using (var writer = File.AppendText("co-op.json"))
            {
                writer.WriteLine(json);
            }
        }
        catch (Exception ex)
        {
            // threw it in a try catch just in case because i/o can give issues sometimes
            Console.WriteLine("Error appending CO-OP Game to JSON file: " + ex.Message);
        }
    }

}