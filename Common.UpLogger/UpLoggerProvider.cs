using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.UpLogger
{
    public class UpLoggerProvider : ILoggerProvider
    {
        public UpLoggerOptions Options { get; private set; }

        public UpLoggerProvider(IOptions<UpLoggerOptions> options)
        {
            Options = options.Value;            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UpLogger(categoryName, this);
        }

        public void Dispose()
        {}
    }
}