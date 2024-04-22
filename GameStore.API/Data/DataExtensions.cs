using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    // must be static because it is a extension method using "this"
    /// <summary>
    /// Execute Migration on start-up
    /// </summary>
    /// <param name="app"></param>
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        // because we did builder.Services.AddSqlite<GameStoreContext>(connString); 
        //      in the Program.cs the program knows about the context so we can call an instance here
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }
}
