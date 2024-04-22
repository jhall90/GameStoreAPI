using GameStore.API.Data;
using GameStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
//add builder configurations before the var app

// get the conn string from the settings
var connString = builder.Configuration.GetConnectionString("GameStore");

//use dependency injection for the connection
// builder.Services.AddSqlite<___INSERT_CONTEXT___>(__INSERT_CONNECTION_STRING__);
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
