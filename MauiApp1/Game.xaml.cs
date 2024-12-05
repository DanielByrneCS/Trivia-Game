using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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




    public Game(int difficulty, int numQuestions, int type)
    {
        InitializeComponent();
        difficult.Text = difficulty.ToString();
        questions.Text = numQuestions.ToString();
        typea.Text = type.ToString();
        httpClient = new HttpClient();
        GameLoop(GetQuestions(difficulty, numQuestions, type));


    }

    private async void GameLoop(Task<string> questions)
    {
       // turns the Task<string> to a string to deserialise the json into question class
       string strQuestions = await questions;
       var deserializedQuestions = JsonSerializer.Deserialize<List<Question>>(strQuestions);

  
    }

    string APILinkCreator(int difficulty, int numQuestions, int questionType)
    {
        //type: MCQ = 0, True/False = 1
        //Difficulty: Easy = 0, Medium = 1, Hard = 2

        return "https://opentdb.com/api.php?amount=" + numQuestions + "&difficulty=" + difficultyLevels[difficulty][1] + "&type=" + typeDict[questionType][1]; 
        

    }

    public async Task<string> GetQuestions(int difficulty, int numQuestions, int questionType)
    {
        var jsonQuestions = await httpClient.GetAsync(APILinkCreator(difficulty, numQuestions, questionType));
        if (jsonQuestions.IsSuccessStatusCode)
        {
            return await jsonQuestions.Content.ReadAsStringAsync();
        }
        return "";
    }
}