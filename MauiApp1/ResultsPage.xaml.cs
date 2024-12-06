namespace MauiApp1;

public partial class ResultsPage : ContentPage
{
	public ResultsPage()
	{
		InitializeComponent();
	}
    public ResultsPage(int QuestionsCorrect, int QuestionsIncorrect, string difficulty , string numberOfQuestions, string questionType)
    {
        InitializeComponent();
        questionsCorrect.Text = QuestionsCorrect.ToString();
        questionsIncorrect.Text = QuestionsIncorrect.ToString();
    }
}