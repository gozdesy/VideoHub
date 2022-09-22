namespace Common.UpLogger
{
    public class UpLoggerOptions 
    {
        public string ConnectionString { get; set; }    

        public string DatabaseName {get; set;}

        public string CollectionName { get; set; }

        public string ApplicationName { get; set; }

        public string Level { get; set;}

        public string[] Fields { get; set; }

        public UpLoggerOptions()
        {
            Level = "Information";
            Fields = new string[0]; 
        }
    }
}