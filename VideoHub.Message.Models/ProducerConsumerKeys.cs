namespace VideoHub.Message.Models
{
    public static class ProducerKey
    {
        public const string VideoUploaded = "VideoUploaded";
        public const string TranscodeCompleted = "TranscodeCompleted";
    }

    public static class ConsumerKey
    {
        public const string TranscodeStart = "TranscodeStart";
        public const string VideoUpdate = "VideoUpdate";
    }
}
