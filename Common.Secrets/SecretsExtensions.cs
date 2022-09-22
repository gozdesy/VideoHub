using Common.Secrets;
using Common.Secrets.SecretsGateway;
using Microsoft.Extensions.Configuration;

namespace Common.Secrets.Extensions;
public static class SecretsExtensions
{
    public static IConfigurationBuilder AddSecretsConfiguration(this IConfigurationBuilder builder, SecretsGatewaysOptions secretsGatewaysOptions)
    {
        return builder.Add(new SecretsConfigurationSource(secretsGatewaysOptions));
    }
}
