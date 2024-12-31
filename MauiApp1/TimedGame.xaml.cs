using Plugin.Maui.Audio;
using System.Text.Json;

namespace MauiApp1;

public partial class TimedGame : ContentPage
{
    private IAudioPlayer correctAudio;
    private IAudioPlayer incorrectAudio;
    private IAudioPlayer ticking;
    private IAudioPlayer riser;
    public TimedGame(int difficulty, int type, string playerName)
    {
        InitializeComponent();
        this.playerName = playerName;
        count = Preferences.Get("TimerLength", 60);
        httpClient = new HttpClient();
        questionType = typeDict[type];
        
        GameLoop(difficulty, type);
        InitialiseTimer();

        DifficultyLabel(difficulty);
        TypeLabel(type);
        timer.Start();
        BindingContext = this;

    }
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
    bool isRunning = false;
    bool riserBool = false;
    public bool IsRunning
    {
        get => isRunning;
        set
        {
            isRunning = value;
            OnPropertyChanged();
            
        }
    }
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
    private HttpClient httpClient;
    QuestionResponse questionResponse;
    string boolOrMCQ;
    Color primaryTextColor;
    Color secondaryBackgroundColor;
    string questionType;
    string playerName;
    int QuestionsCorrect, QuestionsIncorrect, currentQuestion;


    public string Difficulty {  get; set; }

    public string GameType { get; set; }



    
    private void InitialiseTimer()
    {
        timer = new System.Timers.Timer
        {
            Interval = 1000
        };
        timer.Elapsed += (s, e) =>
        {
            ticking.Play();
            if (count < 4 && !riserBool)
            {
                riserBool = true;
                riser.Play();
            }
            --Count;
            if(Count == 0)
                timer.Stop();
            
                
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

   
    

    private async void GameLoop(int difficulty, int questionType)
    {
        if (currentQuestion == 0)
        {
            IsBusy = true;
            ticking = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("ticking.mp3"));
            riser = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("riser.mp3"));
            correctAudio = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("correct.mp3"));
            incorrectAudio = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("incorrect.mp3"));

            await GetQuestions(difficulty, questionType);
            await Task.Delay(1000);
            questionResponse.results[currentQuestion].question = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].question);
            questionTitle.Text = questionResponse.results[currentQuestion].question;
            questionTitle.TranslateTo(0, 1000, 0);
            buttonLayout.TranslateTo(0, 1000, 0);
            questionTitle.TranslateTo(0, 0, 400);
            await buttonLayout.TranslateTo(0, 0, 400);
        }
            
        else
        {
            await Task.Delay(500);
            questionResponse.results[currentQuestion].question = System.Web.HttpUtility.HtmlDecode(questionResponse.results[currentQuestion].question);
            questionTitle.Text = questionResponse.results[currentQuestion].question;
            IsBusy = true;
            
            questionTitle.TranslateTo(0, 1000, 0);
            buttonLayout.TranslateTo(0, 1000, 0);
            questionTitle.TranslateTo(0, 0, 400);
            await buttonLayout.TranslateTo(0, 0, 400);

        }
        buttonLayout.Children.Clear();
        IsBusy = false;
        List<string> possibleAnswers = questionResponse.results[currentQuestion].incorrect_answers;
        possibleAnswers.Add(questionResponse.results[currentQuestion].correct_answer);
        possibleAnswers.Sort();
        if(Preferences.Get("isLightTheme", false))
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
                FontSize = 30
            };
            answer.Clicked += AnswerClicked;
                
            if(j <= 1)
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

        // I kept this because it allows the user to answer the final question before the game ends due to timer
        // or if the user gets 50 questions as thats the api limits
        if (button.Text.Equals(questionResponse.results[currentQuestion].correct_answer))
        {
            correctAudio.Play();
            questionTitle.TranslateTo(3000, 0, 300);
            await buttonLayout.TranslateTo(3000, 0, 300);
            if (questionResponse.results.Count > currentQuestion + 1 && timer.Enabled)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Green;
                currentQuestion++;
                QuestionsCorrect++;
                questionTitle.Text = "";
                GameLoop(1, 1);
            }
            else
            {

                button.BackgroundColor = Colors.Green;
                QuestionsCorrect++;
                questionTitle.Text = "";

                if (questionResponse.results.Count < currentQuestion + 1)
                {
                    GameEnd(true);
                }
                else
                    GameEnd();

            }
        }
        else
        {
            incorrectAudio.Play();
            questionTitle.TranslateTo(0, 1000, 300);
            await buttonLayout.TranslateTo(0, 1000, 300);
            if (questionResponse.results.Count > currentQuestion + 1 && timer.Enabled)
            {
                IsBusy = true;
                button.BackgroundColor = Colors.Red;
                currentQuestion++;
                QuestionsIncorrect++;
                questionTitle.Text = "";
                GameLoop(1, 1);
            }
            else
            {
                button.BackgroundColor = Colors.Red;
                questionTitle.Text = "";
                QuestionsIncorrect++;
                if (questionResponse.results.Count < currentQuestion + 1)
                    GameEnd(true);
                else
                    GameEnd();
            }
        }
    }

    private async void GameEnd(bool ranOut)
    {
        // Occurs when 50 questions are answered as opposed to time running out, person who answered last question will be hot potato
        await Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), playerName, QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType, true));
    }
    private async void GameEnd()
    {
        // int timerLength, string hotPotato, int questionsCorrect, int questionsIncorrect, string difficulty, string questionType, List<string> playerList)
        await Navigation.PushAsync(new ResultsPage(Preferences.Get("TimerLength", 60), playerName, QuestionsCorrect, QuestionsIncorrect, Difficulty, GameType));

    }
    string APILinkCreator(int difficulty, int questionType)
    {
        //type: MCQ = 0, True/False = 1
        //Difficulty: Easy = 0, Medium = 1, Hard = 2
        string url = "https://opentdb.com/api.php?amount=50" + "&difficulty=" + difficultyLevels[difficulty] + "&type=" + typeDict[questionType];

        return url;

    }

    public async Task GetQuestions(int difficulty, int questionType)
    {
        try
        {

            var response = await httpClient.GetAsync(APILinkCreator(difficulty,questionType));
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

