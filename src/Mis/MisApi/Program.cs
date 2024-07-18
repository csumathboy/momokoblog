using csumathboy.Application;
using csumathboy.MisApi.Configurations;
using csumathboy.MisApi.Controllers;
using csumathboy.Infrastructure;
using csumathboy.Infrastructure.Common;
using csumathboy.Infrastructure.Logging.Serilog;
using Serilog;
using Serilog.Formatting.Compact;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Newtonsoft.Json;

[assembly: ApiConventionType(typeof(FSHApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddControllers().AddNewtonsoftJson(option =>
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}