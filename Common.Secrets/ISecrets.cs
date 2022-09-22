namespace Common.Secrets
{
    public interface ISecrets
    {
        public Logger Logger { get; set; }

        public VideoHub VideoHub { get; set; }
    }
}