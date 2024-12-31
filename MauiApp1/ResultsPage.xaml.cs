using System.Security.AccessControl;
using System.Text.Json;
using System.Xml.Linq;

namespace MauiApp1;

public partial class ResultsPage : ContentPage
{
    protected override bool OnBackButtonPressed()
    {
        // Your custom logic here (e.g., show a confirmation dialog)
        return true; // Disable default back button behavior
    }

    // MP Timed Game below (Hot potato)

    public ResultsPage(int timer, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList)
    {
        InitializeComponent();
        resultLabel.Text = "Total questions Correct: " + questionsCorrect;
        resultLabel.Text += "\nTotal Questions Incorrect " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nHot Potato: " + hotPotato + "\nPlayer List:";
        for (int i = 0; i < playerList.Count; i++)
            resultLabel.Text += "\nPlayer " + (i + 1) + ": " + playerList[i];
        resultLabel.Text += "\nTimer Length: " + timer;
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerNames = playerList,
            HotPotato = hotPotato,
            RanOut = false,
            Timer = timer
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "HotPotato.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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
    // Hot potato ran out
    //await Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), names[currentPlayer], QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType, names, ranOut));
    public ResultsPage(int timer, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList, bool ranOut)
    {
        InitializeComponent();
        resultLabel.Text = "Total questions Correct: " + questionsCorrect;
        resultLabel.Text += "\nTotal Questions Incorrect " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nHot Potato: " + hotPotato + "\nPlayer List:";
        for (int i = 0; i < playerList.Count; i++)
            resultLabel.Text += "\nPlayer " + (i + 1) + ": " + playerList[i];
        resultLabel.Text += "\nRan out of time (50 question limit)!";
        resultLabel.Text += "\nTimer Length: " + timer;
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerNames =  playerList,
            HotPotato = hotPotato,
            RanOut = ranOut,
            Timer = timer
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "HotPotato.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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

    // Singleplayer Timed (Time AttacK)
    public ResultsPage(int timer, string name, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType)
    {
        InitializeComponent();
        resultLabel.Text = "Total questions Correct: " + questionsCorrect;
        resultLabel.Text += "\nTotal Questions Incorrect " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nPlayer Name: " + name;
        resultLabel.Text += "\nTimer Length: " + timer;
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerName = name,
            Win = false,
            Timer = timer,
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "TimeAttack.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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
    // Singleplayer Timed (hit 50 questions (win))
    public ResultsPage(int timer, string name, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, bool ranOut)
    {
        InitializeComponent();
        resultLabel.Text = "Total questions Correct: " + questionsCorrect;
        resultLabel.Text += "\nTotal Questions Incorrect " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nPlayer Name: " + name;
        resultLabel.Text += "\nYou won! You hit 50 questions!";
        resultLabel.Text += "\nTimer Length: " + timer;
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerName = name,
            Win = ranOut,
            Timer = timer,

        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "TimeAttack.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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

    // MP Versus
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty , string numberOfQuestions, string questionType, List<string> names, string winner)
    {
        InitializeComponent();
        resultLabel.Text = "Total questions Correct: " + questionsCorrect;
        resultLabel.Text += "\nTotal Questions Incorrect " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nWinner: " + winner;
  
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            PlayerNames = names,
            Winner = winner
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "MPVersus.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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


    // sp Set questions 
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty, string numberOfQuestions, string questionType, string name)
    {
        InitializeComponent();
        resultLabel.Text = "You got " + questionsCorrect + " out of " + numberOfQuestions + " correct!";
        resultLabel.Text += "\nYou got " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "\nPlayer Name: " + name;
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            PlayerName = name,
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "SPSet.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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
    // Gamemode 5 (co-op)
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty, string numberOfQuestions, string questionType, List<string> names, int gameMode)
    {
        InitializeComponent();
        resultLabel.Text = "You got " + questionsCorrect + " out of " + numberOfQuestions + " correct!";
        resultLabel.Text += "\nYou got " + questionsIncorrect + " incorrect!";
        resultLabel.Text += "\nDifficulty: " + difficulty;
        resultLabel.Text += "\nQuestion Type: " + questionType;
        resultLabel.Text += "Player List:";
        for (int i = 0; i < names.Count; i++)
            resultLabel.Text += "\nPlayer " + (i + 1) + ": " + names[i];
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            NumberOfQuestions = numberOfQuestions,
            Players = names,
            Difficulty = difficulty,
            Type = questionType,
            GameMode = gameMode
        };

        // turns the object into json format
        var json = JsonSerializer.Serialize(result);

        // Adds the json to the corresponding gamemodes file
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "co-op.json");
        try
        {
            using (var writer = File.AppendText(targetFile))
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