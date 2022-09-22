using Common.Secrets.SecretsGateway;
using Microsoft.Extensions.Configuration;

namespace Common.Secrets;
public class SecretsConfigurationSource : IConfigurationSource
{
    private readonly SecretsGatewaysOptions _secretsGatewaysOptions;

    public SecretsConfigurationSource(SecretsGatewaysOptions secretsGatewaysOptions) => _secretsGatewaysOptions = secretsGatewaysOptions;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new SecretsConfigurationProvider(_secretsGatewaysOptions);
}
