using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

//  context inherits DbContext
//      pass in a DbContextOptions<> 
//          add in instance of the created context and give it a name like options
//  pass in the given name into DbContext as shown 
public class GameStoreContext(DbContextOptions<GameStoreContext> options)
    : DbContext(options)
{
    // Dbsets are objects that can be used to query and save the instances
    // Dbset<> should be of the type of Entity
    // instead of { get; set; } I use => Set<_____>();
    //      passing in the the correct type 
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    /// <summary>
    /// Executed with the migration execution
    /// Only for simple data, don't use when the data is complex
    /// this doesn't need to interact with any other areas in the application so it is ok to do this for now
    /// only static data
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "MMORPG" },
            new { Id = 2, Name = "Fighting" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "RPG" },
            new { Id = 6, Name = "Kids and Family" },
            new { Id = 7, Name = "Horror" },
            new { Id = 8, Name = "Simulation" },
            new { Id = 9, Name = "Shooter" }
        );
    }
}
