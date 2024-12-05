using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MauiApp1;

public partial class Game : ContentPage
{
    // Dictionary helps convert int difficulty to string for api url creation
    Dictionary<int, string> difficultyLevels = new Dictionary<int, string>
    {
        { 0, "easy" },
        { 1, "medium" },
        { 2, "hard" }
    };
    // Probably not as necessary as there is only 2 values but its good to keep organised and easy to expand
    Dictionary<int, string> typeDict = new Dictionary<int, string>
    {
        { 0, "multiple" },
        { 1, "boolean" },
    };

    private HttpClient httpClient;
    QuestionResponse questionResponse;

    public string Difficulty {  get; set; }
    public string NumberOfQuestions {  get; set; }
    public string GameType { get; set; }
    public Game(int difficulty, int numQuestions, int type)
    {
        InitializeComponent();
        httpClient = new HttpClient();
        GetQuestions(difficulty, numQuestions, type );
        GameLoop(difficulty, numQuestions, type);
        QuestionsLabel(numQuestions);
        DifficultyLabel(difficulty);
        TypeLabel(type);
        BindingContext = this;''

    }

    private void DifficultyLabel(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                Difficulty = "Easy";
                break;
            case 1:
                Difficulty = "Medium";
                break;
            case 2:
                Difficulty = "Hard";
                break;
            default:
                Difficulty = "You shouldn't see this ever";
                break;
        }
    }

    private void TypeLabel(int type)
    {
        switch (type)
        {
            case 0:
                GameType = "Multiple Choice";
                break;
            case 1:
                GameType = "True/False";
                break;
            default:
                GameType = "This shouldn't happen";
                break;
        }
    }

    void QuestionsLabel(int numq)
    {
        switch (numq)
        {
            case 5:
                NumberOfQuestions = "5 Questions";
                break;
            case 10:
                NumberOfQuestions = "10 Questions";
                break;
            case 15:
                NumberOfQuestions = "15 Questions";
                break;
            case 20:
                NumberOfQuestions = "20 Questions";
                break;

            default:
                NumberOfQuestions = "This shouldn't happen";
                break;
        }
    }
    private async void DisplayQuestion()
    {
        await Task.Delay(1000);
        foreach (Question question in questionResponse.results)
        {
            questionTitle.Text += question.question + "\n";
        }
        
    }

    private async void GameLoop(int difficulty, int numQuestions, int type)
    {

        for(int i = 0; i < numQuestions; i++)
        {

        }
       

    }

    string APILinkCreator(int difficulty, int numQuestions, int questionType)
    {
        //type: MCQ = 0, True/False = 1
        //Difficulty: Easy = 0, Medium = 1, Hard = 2

        return "https://opentdb.com/api.php?amount=" + numQuestions + "&difficulty=" + difficultyLevels[difficulty][1] + "&type=" + typeDict[questionType][1]; 
        

    }

    public async Task GetQuestions(int difficulty, int numQuestions, int questionType)
    {
        try
        {
    
            var response = await httpClient.GetAsync("https://opentdb.com/api.php?amount=10&difficulty=medium");
            if (response.IsSuccessStatusCode)
            {
                string contents = await response.Content.ReadAsStringAsync();
                // Below line from Json2CSharp 
                questionResponse = JsonSerializer.Deserialize<QuestionResponse>(contents);
                
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error in loading Questions", ex.Message, "OK");
        }
    
    }


}
