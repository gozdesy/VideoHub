using Common.Secrets.SecretsGateway;

namespace Common.Secrets
{
    public class HashiCorpVaultOptions 
    {
        public string PublicUrl { get; set; }
        public string Token { get; set; }
        public string MountPoint { get; set; }
        public string Path { get; set; }
        public bool TokenRenewSelf { get; set; }
    }
}
