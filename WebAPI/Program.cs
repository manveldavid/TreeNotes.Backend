using Serilog.Events;
using Serilog;
using WebAPI.Configures;

var logDir = "Data/Logs/";
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), logDir));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("Data/Logs/TreeNotesWebApiLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
ConfigureServices.Configure(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureScope.Configure(app.Services.CreateScope());
ConfigurePipeLine.Configure(app);

app.Run();