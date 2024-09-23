
using Practice.WebAPI;

var builder = WebApplication.CreateBuilder(args);
var startUp = new Startup(builder.Configuration,builder.Environment);
startUp.ConfigureServices(builder.Services);

var app = builder.Build();
startUp.Configure(app,builder.Environment);