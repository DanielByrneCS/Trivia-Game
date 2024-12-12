using System.Text.Json;


namespace MauiApp1;

public partial class CoopGame : ContentPage
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
    string boolOrMCQ;
    Color primaryTextColor;
    Color secondaryBackgroundColor;

    int QuestionsCorrect, QuestionsIncorrect, currentQuestion;

    // Strings below are for binding context
    public string Difficulty { get; set; }
    public string NumberOfQuestions { get; set; }
    public string GameType { get; set; }

    // some of the ints below get converted to the word for displaying to user above
    private int numOfPlayers;
    public int currentPlayer;
    private int difficulty;
    private int numQuestions;
    private int questionType;
    List<string> names;


    // 5 is for co-op
    int gameMode = 5;
    
    // I felt another class for multiplayer, although repeating code, was better
    // Game.xaml.cs is big enough to navigate so this will avoid headaches, would be very easy to combine them if need
    public CoopGame(int difficulty, int numQuestions, int questionType, int numOfPlayers, List<string> names)
    {
        this.numOfPlayers = numOfPlayers;
        this.names = names;
        this.difficulty = difficulty;
        this.numQuestions = numQuestions;
        this.questionType = questionType;
        InitializeComponent();
        httpClient = new HttpClient();
        
        GameLoop(currentPlayer);
        QuestionsLabel(numQuestions);
        DifficultyLabel(difficulty);
        TypeLabel(questionType);
        BindingContext = this;

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
                boolOrMCQ = "multiple";
                break;
            case 1:
                GameType = "True/False";
                boolOrMCQ = "boolean";
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


    private async void GameLoop(int currPlayer)
    {
        currentPlayerLabel.Text = "Current Player: " + names[currentPlayer];
        await GetQuestions();
        if (currentQuestion == 0)
        {
            IsBusy = true;
            await Task.Delay(1000);
        }
            
        else
        {
            IsBusy = true;
            await Task.Delay(200);

        }
        
        buttonLayout.Children.Clear();
        questionResponse.results[currentQuestion].question = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].question);
        IsBusy = false;
        questionTitle.Text = questionResponse.results[currentQuestion].question;
        List<string> possibleAnswers = questionResponse.results[currentQuestion].incorrect_answers;
        possibleAnswers.Add(questionResponse.results[currentQuestion].correct_answer);
        possibleAnswers.Sort();
        if (Preferences.Get("isLightTheme", false))
        {
            primaryTextColor = Color.FromArgb("#151515");
            secondaryBackgroundColor = Color.FromArgb("FF6347");
        }
        else
        {
            primaryTextColor = Color.FromArgb("#FFFFFF");
            secondaryBackgroundColor = Color.FromArgb("FF6347");

        }
        HorizontalStackLayout view1 = new HorizontalStackLayout();
        HorizontalStackLayout view2 = new HorizontalStackLayout();
        // Line below gets rid of &nbsp; and all those html formatting things from the json

        questionResponse.results[currentQuestion].correct_answer = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].correct_answer);
        for (int j = 0; j < questionResponse.results[currentQuestion].incorrect_answers.Count; j++)
        {
            questionResponse.results[currentQuestion].incorrect_answers[j] = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].incorrect_answers[j]);
            possibleAnswers[j] = System.Web.HttpUtility.HtmlDecode(possibleAnswers[j]);
            Button answer = new Button
            {
                Text = possibleAnswers[j],
                TextColor = primaryTextColor,
                BackgroundColor = secondaryBackgroundColor,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 30,
                Margin = 10
            };
            answer.Clicked += AnswerClicked;

            if (j <= 1)
                view1.Children.Add(answer);
            else
                view2.Children.Add(answer);


        }
        buttonLayout.Children.Add(view1);
        buttonLayout.Children.Add(view2);



    }
    private async void AnswerClicked(object sender, EventArgs e)
    {

        Button button = (Button)sender;

        StackLayout buttonLayout = (StackLayout)button.Parent.Parent;

        // Below is if user selects correct answer to question
        if (button.Text.Equals(questionResponse.results[currentQuestion].correct_answer))
        {
            if (questionResponse.results.Count > currentQuestion + 1)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Green;
                currentQuestion++;
                QuestionsCorrect++;
                if (currentPlayer == numOfPlayers - 1)
                    currentPlayer = 0;
                else
                    currentPlayer++;
                questionsCor.Text = QuestionsCorrect.ToString();
                questionTitle.Text = "";
                GameLoop(currentPlayer);
            }
            else
            {

                button.BackgroundColor = Colors.Green;
                QuestionsCorrect++;
                questionTitle.Text = "";
                // gameMode is 5 for co-op (can add more gamemodes easily with this)
                GameEnd();
            }
        }
        else
        {
            if (questionResponse.results.Count > currentQuestion + 1)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Red;
                currentQuestion++;
                QuestionsIncorrect++;
                if (currentPlayer == numOfPlayers - 1)
                    currentPlayer = 0;
                else
                    currentPlayer++;
                questionsIncor.Text = QuestionsIncorrect.ToString();
                questionTitle.Text = "";
                GameLoop(currentPlayer);
            }
            else
            {
                button.BackgroundColor = Colors.Red;
                QuestionsIncorrect++;
                questionTitle.Text = "";
                // gameMode is 5 for co-op (can add more gamemodes easily with this)
                GameEnd();
                    

            }
        }
    }

    private async void GameEnd()
    {
        await Navigation.PushAsync(new ResultsPage(QuestionsCorrect, QuestionsIncorrect, names, gameMode));

    }


    string APILinkCreator(int difficulty, int numQuestions, int questionType)
    {
        //type: MCQ = 0, True/False = 1
        //Difficulty: Easy = 0, Medium = 1, Hard = 2
        string url = "https://opentdb.com/api.php?amount=" + numQuestions + "&difficulty=" + difficultyLevels[difficulty] + "&type=" + typeDict[questionType];

        return url;

    }

    public async Task GetQuestions()
    {
        try
        {

            var response = await httpClient.GetAsync(APILinkCreator(difficulty, numQuestions, questionType));
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
