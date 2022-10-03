using Microsoft.Extensions.DependencyInjection;

namespace VideoHub.Transcode
{
    public static class TranscodeManagerExtensions
    {
        public static IServiceCollection AddTranscodeManager(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(ITranscodeManager), typeof(TranscodeManager));
            return serviceCollection;
        }

        public static void InitializeTranscodeManager(this IServiceProvider serviceProvider)
        {
            var transcodeManager = (TranscodeManager) serviceProvider.GetRequiredService<ITranscodeManager>();
            transcodeManager.SetMessageReceivers();
        }
    }
}
