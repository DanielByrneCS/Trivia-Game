namespace MauiApp1;

public partial class Gamemodes : ContentPage
{
  
    // Eventhandler for mainpage to await for the selected gamemode that is to be sent back
    public event EventHandler<int> DataSentBack;
    public Gamemodes(int difficulty, int numQuestions, int questionType, int numPlayers)
	{
		// 1 and 3 must return timer not questions
		InitializeComponent();
		if (numPlayers > 1)
		{
            // Disables the singleplayer only gamemode
			gm1.IsEnabled = false;
            // unhides label to state unavailable with multiplayer
            gm1L.IsVisible = true;
            gm1.BackgroundColor = (Color)Application.Current.Resources["TertiaryBackgroundColor"];
        }
		else
		{
			gm3.IsEnabled = false;
			gm4.IsEnabled = false;
            gm3L.IsVisible = true;
            gm4L.IsVisible = true;
            gm3.Text += "\n(Disabled in Single Player)";
            gm4.Text += "\n(Disabled in Single Player)";
            gm3.BackgroundColor = (Color)Application.Current.Resources["TertiaryBackgroundColor"];
            gm4.BackgroundColor = (Color)Application.Current.Resources["TertiaryBackgroundColor"];
            gm2.Text = "2: Set Questions";
		}

    }

    private async void gmClick(object sender, EventArgs e)
    {
		Button button = (Button)sender;
		if (button.Text.Contains("1:"))
		{
            gm1.BackgroundColor = Colors.Green;
            DataSentBack?.Invoke(this, 1);
            await Navigation.PopAsync();
        }
        else if (button.Text.Contains("2:"))
		{
            gm2.BackgroundColor = Colors.Green;
            DataSentBack?.Invoke(this, 2);
            await Navigation.PopAsync();
        }
        else if (button.Text.Contains("3:"))
        {
            gm3.BackgroundColor = Colors.Green;
            DataSentBack?.Invoke(this, 3);
            await Navigation.PopAsync();
        }
        else if (button.Text.Contains("4:"))
        {
            gm4.BackgroundColor = Colors.Green;
            DataSentBack?.Invoke(this, 4);
            await Navigation.PopAsync();
        }
    }
}