namespace MauiApp1;

public partial class Settings : ContentPage
{
    private bool isLightTheme = true;
	public Settings()
	{
		InitializeComponent();
	}

    private void ThemeButton(object sender, EventArgs e)
    {
        // Below link helped with some c# for the themes
        // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/theming?view=net-maui-9.0
        
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
}