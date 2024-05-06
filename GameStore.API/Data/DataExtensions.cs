using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    // must be static because it is a extension method using "this"
    /// <summary>
    /// Execute Migration on start-up
    /// Since I am returning a task with Async I am no longer using public static void MigrateDb
    /// instead it is swapped to public static async Task MigrateDbAsync
    /// </summary>
    /// <param name="app"></param>
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        // because we did builder.Services.AddSqlite<GameStoreContext>(connString); 
        //      in the Program.cs the program knows about the context so we can call an instance here
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();
    }
}
