using SQLite;
using LocationHeatMap.Models;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<LocationEntry>().Wait();
    }

    public Task<int> InsertLocationAsync(LocationEntry entry) =>
        _db.InsertAsync(entry);

    public Task<List<LocationEntry>> GetLocationsAsync() =>
        _db.Table<LocationEntry>().ToListAsync();
}