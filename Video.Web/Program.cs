using VideoHub.Web.Models;
using Common.UpLogger;
using Common.Secrets.Extensions;
using Common.Secrets;
using Common.Secrets.SecretsGateway;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionHandler(options => { });
builder.Services.Configure<VideoHubWebSettings>(builder.Configuration.GetSection("VideoHubWebSettings"));

// SECRETS
// -----------------------------------------------------------------------------------------------------------------------------------------
builder.Configuration.AddSecretsConfiguration(builder.Configuration.GetSection("SecretsGatewaysOptions").Get<SecretsGatewaysOptions>());

// LOGGER
// -----------------------------------------------------------------------------------------------------------------------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddUpLogger(options => {
    builder.Configuration.GetSection(nameof(UpLogger)).Bind(options);
    options.ConnectionString = builder.Configuration[Secrets.LoggerMongoConnectionStringKey];
});

// Add services to the container.
var mvc = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    mvc.AddRazorRuntimeCompilation();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
