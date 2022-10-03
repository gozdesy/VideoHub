using MongoDB.Driver;
using VideoHub.Nosql.Data;
using VideoHub.API.Services;
using Common.UpLogger;
using Common.Secrets;
using Common.Secrets.Extensions;
using Common.Secrets.SecretsGateway;
using VideoHub.Transcode;
using Common.MessageQueueManager.Extensions;
using Common.MessageQueueManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler(options => { });

builder.Configuration.AddSecretsConfiguration(builder.Configuration.GetSection("SecretsGatewaysOptions").Get<SecretsGatewaysOptions>());

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddUpLogger(options => { 
    builder.Configuration.GetSection(nameof(UpLogger)).Bind(options);
    options.ConnectionString = builder.Configuration[Secrets.LoggerMongoConnectionStringKey];
});

builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration[Secrets.VideoHubMongoConnectionStringKey]));

var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.AddScoped<IMongoDatabase>(provider => 
{ 
    var client = provider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

var messageQueueOptions = builder.Configuration.GetSection("MessageQueueOptions").Get<MessageQueueOptions>();
var videoContext = new VideoHubContext(messageQueueOptions.ProjectName);
builder.Services.AddSingleton<VideoHubContext>(videoContext);

// MESSAGE QUEUE MANAGER
builder.Services.AddMessageQueueManager(options =>
    {
        builder.Configuration.GetSection("MessageQueueOptions").Bind(options);
        options.Uri = builder.Configuration[Secrets.MessageQueueManagerUriKey];  
    }, 
    videoContext.ProducerConfigs
);

// TRANSCODE MANAGER
builder.Services.AddTranscodeManager();

// MESSAGE LISTENER
builder.Services.AddSingleton(typeof(IMessageListener), typeof(MessageListener));

// SERVICES
builder.Services.AddScoped(typeof(IVideoDbContext), typeof(VideoDbContext));

builder.Services.AddScoped(typeof(IVideoRepository), typeof(VideoRepository));

builder.Services.AddScoped(typeof(IVideoService), typeof(VideoService));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// INITIALIZE TRANSCODE MANAGER
app.Services.InitializeTranscodeManager();

// INITIALIZE MESSAGE LISTENER
var messageListener = (MessageListener)app.Services.GetRequiredService<IMessageListener>();
messageListener.SetMessageReceivers();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
