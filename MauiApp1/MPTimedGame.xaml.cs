using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Plugin.Maui.Audio;


namespace MauiApp1;

public partial class MPTimedGame : ContentPage
{
    private IAudioPlayer correctAudio;
    private IAudioPlayer incorrectAudio;


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
    bool ranOut = false;

    int QuestionsCorrect, QuestionsIncorrect, currentQuestion;


    public string Difficulty { get; set; }
    public string NumberOfQuestions { get; set; }
    public string GameType { get; set; }

    private System.Timers.Timer timer;
    int count = 0;
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            OnPropertyChanged();
        }
    }

    List<string> names;
    private int numOfPlayers;
    public int currentPlayer;
    private int difficulty;
    private int numQuestions;
    private int questionType;
    
    // I felt another class for multiplayer/timed, although repeating code, was better
    // Game.xaml.cs is big enough to navigate so this will avoid headaches, would be very easy to combine them if need
    public MPTimedGame(int difficulty, int questionType, int numOfPlayers, List<string> names)
    {

        this.numOfPlayers = numOfPlayers;
        this.difficulty = difficulty;
        numQuestions = 50;
        this.questionType = questionType;
        this.names = names;
        InitializeComponent();
        httpClient = new HttpClient();
        GetQuestions();
        InitialiseTimer();
        GameLoop(currentPlayer);
        QuestionsLabel(numQuestions);
        DifficultyLabel(difficulty);
        TypeLabel(questionType);
        count = Preferences.Get("TimerLength", 60);
        timer.Start();
        BindingContext = this;

    }


    private void InitialiseTimer()
    {
        timer = new System.Timers.Timer
        {
            Interval = 1000
        };
        timer.Elapsed += (s, e) =>
        {
            --Count;
            if (Count == 0)
            {
                timer.Stop();
                GameEnd();
                
            }
                


        };

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
        if (currentQuestion == 0)
        {
            questionResponse.results[currentQuestion].question = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].question);
            questionTitle.Text = questionResponse.results[currentQuestion].question;
            IsBusy = true;
            // Gives time to load
            await Task.Delay(1000);
            correctAudio = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("correct.mp3"));
            incorrectAudio = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("incorrect.mp3"));
            
            await questionTitle.TranslateTo(0, 1000, 0);
            buttonLayout.TranslateTo(0, 1000, 0);
            questionTitle.TranslateTo(0, 0, 400);
            buttonLayout.TranslateTo(0, 0, 400); 
            
        }
            
        else
        {
            questionResponse.results[currentQuestion].question = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].question);
            questionTitle.Text = questionResponse.results[currentQuestion].question;
            await Task.Delay(400);
            
            questionTitle.TranslateTo(0, 1000, 0);
            buttonLayout.TranslateTo(0, 1000, 0);
            questionTitle.TranslateTo(0, 0, 400);
            await buttonLayout.TranslateTo(0, 0, 400);

        }
        // Line below turns things like &nbsp into what it should be (apostrophes etc)
       
        buttonLayout.Children.Clear();
        IsBusy = false;
        
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
            // Animation for a Correct Answer
            correctAudio.Play();
            questionTitle.TranslateTo(1000, 0, 300);
            await buttonLayout.TranslateTo(1000, 0, 300);

            if (questionResponse.results.Count > currentQuestion + 1)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Green;
                currentQuestion++;
                QuestionsCorrect++;
                if(currentPlayer >= numOfPlayers -1)
                    currentPlayer = 0;
                else
                    currentPlayer++;
                questionsCor.Text = QuestionsCorrect.ToString();
                
                GameLoop(currentPlayer);
            }
            else
            {

                button.BackgroundColor = Colors.Green;
                QuestionsCorrect++;
                
                if (currentPlayer >= numOfPlayers -1)
                    GameEnd(QuestionsCorrect, QuestionsIncorrect);
                else
                {
                    await GetQuestions();
                    currentPlayer++;
                    currentQuestion = 0;
                    GameLoop(currentPlayer);
                    
                }
            }
        }
        else
        {
            incorrectAudio.Play();
            // Animation for an Incorrect Answer
            questionTitle.TranslateTo(0, 1000, 300);
            await buttonLayout.TranslateTo(0, 1000, 300);
            if (questionResponse.results.Count > currentQuestion + 1)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Red;
                currentQuestion++;
                QuestionsIncorrect++;
                questionsIncor.Text = QuestionsIncorrect.ToString();
                
                GameLoop(currentPlayer);
            }
            else
            {
                button.BackgroundColor = Colors.Red;
                QuestionsIncorrect++;
                
                if (currentPlayer == numOfPlayers)
                {
                    ranOut = true;
                    GameEnd(QuestionsCorrect, QuestionsIncorrect);
                }
                    
                else
                {
                    await GetQuestions();
                    currentPlayer++;
                    currentQuestion = 0;
                    GameLoop(currentPlayer);
                }
                    
                    

            }
        }
    }

    private async void GameEnd(int questionsCorrect, int questionsIncorrect)
    {
        // Occurs when 50 questions are answered as opposed to time running out, person who answered last question will be hot potato
        // Had to use the main thread for the new page as i would get a COMException otherwise, not too sure why but seen this on ms docs.
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), names[currentPlayer], QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType, names, ranOut));
        });
    }
    private async void GameEnd()
    {
        // int timerLength, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList)
        await Task.Delay(2000);
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), names[currentPlayer], QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType, names));

        });
    }

    string APILinkCreator(int difficulty, int questionType)
    {
        //type: MCQ = 0, True/False = 1
        //Difficulty: Easy = 0, Medium = 1, Hard = 2
        string url = "https://opentdb.com/api.php?amount=50" + "&difficulty=" + difficultyLevels[difficulty] + "&type=" + typeDict[questionType];

        return url;

    }

    public async Task GetQuestions()
    {
        try
        {

            var response = await httpClient.GetAsync(APILinkCreator(difficulty, questionType));
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
