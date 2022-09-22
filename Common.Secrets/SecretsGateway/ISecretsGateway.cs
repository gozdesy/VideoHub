namespace Common.Secrets.SecretsGateway
{
    public interface ISecretsGateway
    {
        ISecrets GetSecrets();
    }
}
