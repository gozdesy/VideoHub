using Newtonsoft.Json;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace Common.Secrets.SecretsGateway
{
    public sealed class HashiCorpVault : ISecretsGateway
    {
        HashiCorpVaultOptions _options { get; }

        public HashiCorpVault(HashiCorpVaultOptions options)
        {
            _options = options;

            if (_options == null)
            {
                throw new ArgumentNullException(nameof(_options));
            }
        }

        public ISecrets GetSecrets()
        {
            ISecrets secrets = new Secrets();

            try
            {
                IAuthMethodInfo authMethod = new TokenAuthMethodInfo(_options.Token);
                var vaultClientSettings = new VaultClientSettings(_options.PublicUrl, authMethod);
                IVaultClient vaultClient = new VaultClient(vaultClientSettings);
                
                if (_options.TokenRenewSelf) {
                    vaultClient.V1.Auth.Token.RenewSelfAsync().Wait();
                }
                
                var vaultResult = vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(path: _options.Path, mountPoint: _options.MountPoint).Result;
                if (vaultResult != null && vaultResult.Data != null) 
                {
                    var data = vaultResult.Data.FirstOrDefault();
                    secrets = JsonConvert.DeserializeObject<Secrets>(data.Value.ToString());
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }

            return secrets;
        }

    }
}
