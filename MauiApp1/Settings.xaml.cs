namespace MauiApp1;

public partial class Settings : ContentPage
{
    private bool isLightTheme = true;
    private int timerLength;
    public int TimerLength
    {
        get => timerLength;
        set
        {
            if (timerLength != value)
            {
                timerLength = value;
                OnPropertyChanged(nameof(TimerLength));
            }
        }
    }
    public Settings()
	{
		InitializeComponent();
        LoadTimerLength();
        BindingContext = this;
	}

    private void ThemeButton(object sender, EventArgs e)
    {
        // Below link helped with some c# for the themes
        // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/theming?view=net-maui-9.0
        isLightTheme = Preferences.Get("isLightTheme", false);
        if (!isLightTheme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
            Preferences.Set("isLightTheme", true);
            isLightTheme = true;

        }
        else
        {
          
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            Preferences.Set("isLightTheme", false);
            isLightTheme = false;
        }


    }

   

    private void LoadTimerLength()
    {
        int savedTimerLength = Preferences.Get("TimerLength", 60);
        TimerLength = savedTimerLength;
    }

    

    private void TimerButton(object sender, EventArgs e)
    {
        // Im aware this limits the timer to 4 options but it allows the user to not always be typing and looks clean
        Button button = (Button)sender;
        if (button.Text.Contains("30"))
        {
            TimerLength = 30;
            Preferences.Set("TimerLength", 30);
        }
        else if (button.Text.Contains("60"))
        {
            TimerLength = 60;
            Preferences.Set("TimerLength", 60);
        }
        else if (button.Text.Contains("90"))
        {
            TimerLength = 90;
            Preferences.Set("TimerLength", 90);
        }
        else
        {
            Preferences.Set("TimerLength", 120);
            TimerLength = 120;
        }


    }
}