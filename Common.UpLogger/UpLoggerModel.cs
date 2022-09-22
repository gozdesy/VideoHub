using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.UpLogger
{
    public class UpLoggerModel
    {
        [BsonId]
        [BsonRepresentation((BsonType.ObjectId))]
        public string Id { get; set; }

        public string ApplicationName { get; set; }

        public DateTime LoggedOnUtc  { get; set; }

        public string LogLevel { get; set; }

        public string CategoryName { get; set; }

        public int? ThreadId { get; set; }

        public int? EventId { get; set; }

        public string EventName { get; set; }

        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string ExceptionSource { get; set; }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, this.GetType().GetFields().ToList().Select(p => p.Name + " " + p.GetValue(this).ToString()));
        }
    }
}