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
}
