namespace Common.Secrets.SecretsGateway
{
    public class SecretsGatewaysOptions
    {
        public bool UseGateway { get; set; }
        public string ActiveTypeName { get; set; }
        public HashiCorpVaultOptions HashiCorpVault { get; set; }
    }
}
