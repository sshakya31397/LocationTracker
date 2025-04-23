namespace LocationHeatMap
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent(); // This links to App.xaml
            MainPage = appShell;   // DI-injected shell
        }
    }
}