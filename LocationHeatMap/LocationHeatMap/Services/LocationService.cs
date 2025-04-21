using Microsoft.Maui.Devices.Sensors;
using LocationHeatMap.Models;
using System.Diagnostics;

public class LocationService
{
    private readonly DatabaseService _dbService;

    public LocationService(DatabaseService db)
    {
        _dbService = db;
    }

    public async Task SaveCurrentLocationAsync()
    {
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
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Location error: {ex.Message}");
        }
    }
}