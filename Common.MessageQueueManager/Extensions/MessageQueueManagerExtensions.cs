using Microsoft.Extensions.DependencyInjection;

namespace Common.MessageQueueManager.Extensions
{
    public static class MessageQueueManagerExtensions
    {
        public static IServiceCollection AddMessageQueueManager(this IServiceCollection services, Action<MessageQueueOptions> configure, List<IProducerConfig> producerConfigs) 
        {
            services.Configure<MessageQueueOptions>(configure);
            
            services.AddSingleton(producerConfigs);

            services.AddSingleton(typeof(IMessageQueueManagerFactory), typeof(MessageQueueManagerFactory));

            services.AddSingleton(typeof(IMessageQueueManager), serviceProvider => {
                return serviceProvider.GetRequiredService<IMessageQueueManagerFactory>().Create();
            });

            return services;
        }
    }
}
