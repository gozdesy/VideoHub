using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Common.UpLogger
{
    public class UpLogger : ILogger
    {
        private readonly UpLoggerProvider _provider;
        private readonly string _categoryName;
        private string[] _fields = { "ApplicationName", "LogLevel", "ThreadId", "EventId", "EventName", "Message", "ExceptionMessage", "ExceptionStackTrace", "ExceptionSource", "CategoryName" };
        private IMongoCollection<UpLoggerModel> _collection;

        public UpLogger(string categoryName, UpLoggerProvider provider)
        {
            _provider = provider;
            _categoryName = categoryName;

            if (string.IsNullOrEmpty(_provider.Options.ConnectionString)) { throw new ArgumentNullException(_provider.Options.ConnectionString); }
            if (string.IsNullOrEmpty(_provider.Options.DatabaseName)) { throw new ArgumentNullException(_provider.Options.DatabaseName); }

            var client = new MongoClient(_provider.Options.ConnectionString);
            var database = client.GetDatabase(_provider.Options.DatabaseName);

            var collectionName = String.IsNullOrEmpty(_provider.Options.CollectionName) ? "Logs" : _provider.Options.CollectionName;
            _collection = database.GetCollection<UpLoggerModel>(collectionName);
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return Enum.TryParse(typeof(LogLevel), _provider.Options.Level, out object level) && (byte)((LogLevel)level) <= (byte)logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            try
            {
                if (!IsEnabled(logLevel))
                    return;

                var threadId = Thread.CurrentThread.ManagedThreadId;

                var fields = _provider.Options.Fields.Length == 0 ? _fields : _provider.Options.Fields;

                var values = new UpLoggerModel();
                values.LoggedOnUtc = DateTime.UtcNow;

                foreach (var logField in fields)
                {
                    switch (logField)
                    {
                        case "ApplicationName":
                            values.ApplicationName = _provider.Options.ApplicationName; 
                            break;

                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            {
                                values.LogLevel = logLevel.ToString();
                            }
                            break;
                        case "ThreadId":
                            values.ThreadId = threadId;
                            break;
                        case "EventId":
                            values.EventId = eventId.Id;
                            break;
                        case "EventName":
                            if (!string.IsNullOrWhiteSpace(eventId.Name))
                            {
                                values.EventName = eventId.Name;
                            }
                            break;
                        case "Message":
                            if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                            {
                                values.Message = formatter(state, exception);
                            }
                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                            {
                                values.ExceptionMessage = exception?.Message;
                            }
                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                            {
                                values.ExceptionStackTrace = exception?.StackTrace;
                            }
                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                            {
                                values.ExceptionSource = exception?.Source;
                            }
                            break;
                        case "CategoryName":
                            values.CategoryName = _categoryName;
                            break;
                    }
                }
                
                WriteToConsole(values);
                WriteToDb(values);
            }
            catch (Exception ex)
            {
                WriteToConsole(ex.Message);
            }
            
        }

        private void WriteToConsole(UpLoggerModel model) 
        {
            Console.WriteLine(model.ToString());
        }
        
        private void WriteToConsole(string s)
        {
            Console.WriteLine(s);
        }

        private void WriteToDb(UpLoggerModel model)
        {
            _collection.InsertOne(model);
        }

    }
}