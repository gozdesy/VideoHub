using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.UpLogger
{
    public static class UpLoggerExtensions
    {
        public static ILoggingBuilder AddUpLogger(this ILoggingBuilder builder, Action<UpLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, UpLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}