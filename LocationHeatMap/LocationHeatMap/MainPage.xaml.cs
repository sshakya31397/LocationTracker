using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using LocationHeatMap.Models;
using Microsoft.Maui.ApplicationModel;
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

            // Draw polyline trail
            if (entries.Count > 1)
            {
                var polyline = new Polyline
                {
                    StrokeColor = Colors.Red,
                    StrokeWidth = 5
                };

                foreach (var loc in entries)
                {
                    polyline.Geopath.Add(new Location(loc.Latitude, loc.Longitude));
                }

                MyMap.MapElements.Add(polyline);
            }

            // Also draw individual circles for each point (optional)
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

        private async void OnMapClicked(object sender, MapClickedEventArgs e)
{
    var position = e.Location;

    var entry = new LocationEntry
    {
        Latitude = position.Latitude,
        Longitude = position.Longitude,
        Timestamp = DateTime.UtcNow
    };

    await _dbService.InsertLocationAsync(entry);

    var circle = new Circle
    {
        Center = new Location(entry.Latitude, entry.Longitude),
        Radius = new Distance(30f),
        StrokeColor = Colors.Blue,
        StrokeWidth = 1,
        FillColor = new Color(0, 0, 1, 0.2f)
    };

    MyMap.MapElements.Add(circle);

    await DisplayAlert("Saved!", $"Lat: {position.Latitude}, Lon: {position.Longitude}", "OK");
}

        private async void OnSaveLocationClicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Location permission is required to track your position.", "OK");
                return;
            }

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync()
                               ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
                               await DisplayAlert("Saving", $"Lat: {location.Latitude}, Lon: {location.Longitude}", "OK");

                if (location != null)
                {
                    var entry = new LocationEntry
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };

                    await _dbService.InsertLocationAsync(entry);

                    // Add new point to map
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
                    await DisplayAlert("Location Error", "Unable to get your location.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}