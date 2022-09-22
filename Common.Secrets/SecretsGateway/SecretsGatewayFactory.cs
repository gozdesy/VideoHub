namespace Common.Secrets.SecretsGateway
{
    public class SecretsGatewayFactory
    {
        public ISecretsGateway Create(SecretsGatewaysOptions options)
        {
            if (!Enum.TryParse<SecretsGatewayType>(options.ActiveTypeName, out SecretsGatewayType type))
                type = SecretsGatewayType.HashiCorpVault;
            return Create(type, options);
        }
        public ISecretsGateway Create(SecretsGatewayType type, SecretsGatewaysOptions options) 
        {
            switch (type) 
            {
                case SecretsGatewayType.HashiCorpVault:
                    return new HashiCorpVault(options.HashiCorpVault);
                default:
                    return null;
            }
        }
    }
}
