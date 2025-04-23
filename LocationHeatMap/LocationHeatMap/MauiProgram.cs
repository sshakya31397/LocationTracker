using Microsoft.Extensions.Logging;
using SQLitePCL; // Required for Batteries.Init()
using System.IO;

namespace LocationHeatMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // ✅ Fix: Initialize SQLite native libraries
            Batteries.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register the DatabaseService with SQLite path
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
            builder.Services.AddSingleton(new DatabaseService(dbPath));
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}