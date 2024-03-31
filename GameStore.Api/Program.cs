using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Middleware;
using GameStore.Api.Repository;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString);
builder.Services.AddScoped<GameStoreRepository>();

builder.Services.AddHttpLogging(o => {});

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Structured Logging Set Up
builder.Logging.AddJsonConsole(
    options => {
        options.JsonWriterOptions = new()
        {
            Indented = true,
        };
    }
);

var app = builder.Build();

// app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseExceptionHandler(exceptionHanderApp => exceptionHanderApp.ConfigureExceptionHandler());

app.UseHttpLogging();

app.UseAuthorization(); 

app.MapGameStoreEndpoints();

app.MapGenreEndpoints();

await app.MigrateDbAsync();

app.Run();
