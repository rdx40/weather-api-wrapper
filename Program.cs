// //creates webapp builder
// var builder = WebApplication.CreateBuilder(args);
// //adds controllers that handle web requests, or apis and so
// builder.Services.AddControllers();
// //builds the app
// var app = builder.Build();
// //maps the controllers to the app
// app.MapControllers();
// //runs the app
// app.Run();

using WeatherApiWrapper.Services;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); // must match containerPort in YAML
});

builder.Services.AddControllers();
builder.Services.AddHttpClient<WeatherService>(); // Add this line

var app = builder.Build();

app.MapControllers();

app.Run();
