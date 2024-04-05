using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Middleware;
using GameStore.Api.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlite<GameStoreContext>(builder.Configuration.GetConnectionString("GameStore"));
builder.Services.AddScoped<GameStoreRepository>();

builder.Services.AddHttpLogging(o => {});

builder.Services.AddCors();

builder.Services.AddAuthentication().AddJwtBearer();
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

app.UseCors(options =>
{
    options.WithOrigins("*");
});

// Exception Middleware
// app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseExceptionHandler(exceptionHanderApp => exceptionHanderApp.ConfigureExceptionHandler());

app.UseHttpLogging();

app.UseAuthorization(); 

app.MapGameStoreEndpoints();

app.MapGenreEndpoints();

await app.MigrateDbAsync();

app.Run();
