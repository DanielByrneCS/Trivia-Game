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

    public ResultsPage(int QuestionsCorrect, int QuestionsIncorrect, string difficulty , string numberOfQuestions, string questionType)
    {
        InitializeComponent();
        questionsCorrect.Text += QuestionsCorrect.ToString();
        questionsIncorrect.Text += QuestionsIncorrect.ToString();
    }

    public ResultsPage(int QuestionsCorrect, int QuestionsIncorrect, string difficulty, string numberOfQuestions, string questionType, int timer, int numberOfPlayers)
    {
        InitializeComponent();
        questionsCorrect.Text += QuestionsCorrect.ToString();
        questionsIncorrect.Text += QuestionsIncorrect.ToString();
    }
    public ResultsPage(int QuestionsCorrect, int QuestionsIncorrect, int timer, int numberOfPlayers)
    {
        InitializeComponent();
        questionsCorrect.Text += QuestionsCorrect.ToString();
        questionsIncorrect.Text += QuestionsIncorrect.ToString();
    }

    public ResultsPage(int QuestionsCorrect, int QuestionsIncorrect, string difficulty, string numberOfQuestions, string questionType, int numberOfPlayers)
    {
        InitializeComponent();
        questionsCorrect.Text += QuestionsCorrect.ToString();
        questionsIncorrect.Text += QuestionsIncorrect.ToString();
    }
}