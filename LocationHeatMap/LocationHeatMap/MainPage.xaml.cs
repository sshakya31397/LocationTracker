using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using LocationHeatMap.Models;
using Microsoft.Maui.ApplicationModel; // <-- For Permissions and Geolocation
using Microsoft.Maui.Devices.Sensors;

namespace LocationHeatMap
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _dbService;

        public MainPage(DatabaseService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var entries = await _dbService.GetLocationsAsync();

            foreach (var loc in entries)
            {
                var circle = new Circle
                {
                    Center = new Location(loc.Latitude, loc.Longitude),
                    Radius = new Distance(30f),
                    StrokeColor = Colors.Red,
                    StrokeWidth = 1,
                    FillColor = new Color(1, 0, 0, 0.2f)
                };

                MyMap.MapElements.Add(circle);
            }
        }

        private async void OnSaveLocationClicked(object sender, EventArgs e)
        {
            // ✅ Request permission
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Location permission is required to track your position.", "OK");
                return;
            }

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    var entry = new LocationEntry
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };

                    await _dbService.InsertLocationAsync(entry);

                    var circle = new Circle
                    {
                        Center = new Location(entry.Latitude, entry.Longitude),
                        Radius = new Distance(30f),
                        StrokeColor = Colors.Red,
                        StrokeWidth = 1,
                        FillColor = new Color(1, 0, 0, 0.2f)
                    };

                    MyMap.MapElements.Add(circle);
                }
                else
                {
                    await DisplayAlert("Location Error", "Unable to get location.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}