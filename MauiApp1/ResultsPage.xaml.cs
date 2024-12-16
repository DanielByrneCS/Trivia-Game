using System.Security.AccessControl;
using System.Text.Json;
using System.Xml.Linq;

namespace MauiApp1;

public partial class ResultsPage : ContentPage
{
	

    // MP Timed Game below (Hot potato)
    
    public ResultsPage(int timer, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList)
    {
        InitializeComponent();
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerList = playerList,
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
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            Difficulty = difficulty,
            QuestionType = questionType,
            PlayerList =  playerList,
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
        questionsCorrectLabel.Text += questionsIncorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
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
        questionsCorrectLabel.Text += questionsIncorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
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
    public ResultsPage(int questionsCorrect, int questionsIncorrect, string difficulty , string numberOfQuestions, string questionType, List<string> names)
    {
        InitializeComponent();
        questionsCorrectLabel.Text += questionsCorrect.ToString();
        questionsIncorrectLabel.Text += questionsIncorrect.ToString();
        // Creates object to append to json
        var result = new
        {
            CorrectAnswers = questionsCorrect,
            IncorrectAnswers = questionsIncorrect,
            PlayerNames = names,
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