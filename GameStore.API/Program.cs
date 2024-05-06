using GameStore.API.Data;
using GameStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
//add builder configurations before the var app

// get the conn string from the settings
var connString = builder.Configuration.GetConnectionString("GameStore");
//  dbContext is not Thread safe
//      having a single instance instead of scoped across multiple requests could result in concurrency issues
//      reusing the same dbContext across multiple requests could lead to memory usage increase
//          This is because dbContext keeps track of changes over its lifetime


//use dependency injection for the connection
// builder.Services.AddSqlite<___INSERT_CONTEXT___>(__INSERT_CONNECTION_STRING__);
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
