namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private Button selectedButton;
        public MainPage()
        {
            InitializeComponent();
            selectedButton = buttonLayout.Children.OfType<Button>().FirstOrDefault(b => b.Text == "10 Questions");
            selectedButton.BackgroundColor = Colors.Green;
        }
        private void NumberQuestions(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (selectedButton != null && selectedButton != button)
            {
                
                selectedButton.BackgroundColor = Colors.AliceBlue;
                selectedButton.TextColor = Colors.Navy;
            }

            selectedButton = button;
            selectedButton.BackgroundColor = Colors.Green;
            selectedButton.TextColor = Colors.White;
        }


    }

}
