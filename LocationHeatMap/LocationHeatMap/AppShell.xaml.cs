namespace LocationHeatMap
{
    public partial class AppShell : Shell
    {
        public AppShell(MainPage mainPage)
        {
            InitializeComponent();

            // Register route or assign as shell content
            Items.Add(new ShellContent
            {
                Title = "MainView",
                Content = mainPage
            });
        }
    }
}