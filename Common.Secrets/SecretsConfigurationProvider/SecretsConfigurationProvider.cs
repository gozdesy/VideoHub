using Common.Secrets.SecretsGateway;
using Microsoft.Extensions.Configuration;

namespace Common.Secrets;

public class SecretsConfigurationProvider : ConfigurationProvider
{
    SecretsGatewaysOptions _secretsGatewaysOptions { get; }

    public SecretsConfigurationProvider(SecretsGatewaysOptions secretsGatewaysOptions)
    {
        _secretsGatewaysOptions = secretsGatewaysOptions;
    }

    public override void Load()
    {
        if (_secretsGatewaysOptions.UseGateway) {
            ISecretsGateway secretsGateway = new SecretsGatewayFactory().Create(_secretsGatewaysOptions);
            var secrets = secretsGateway.GetSecrets();

            Data.Add(Secrets.LoggerMongoConnectionStringKey, secrets.Logger.MongoConnectionString);
            Data.Add(Secrets.VideoHubMongoConnectionStringKey, secrets.VideoHub.MongoConnectionString);
        }
    }
}


