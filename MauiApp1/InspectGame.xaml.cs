namespace MauiApp1;

public partial class InspectGame : ContentPage
{

    public InspectGame(string text)
    {
        InitializeComponent();
        Text = text;
        BindingContext = this;
    }

    public string Text { get; }
}