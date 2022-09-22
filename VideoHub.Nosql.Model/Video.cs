using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VideoHub.Nosql.Model
{
    //[BsonIgnoreExtraElements]
    public class Video : BaseModel
    {
        [BsonId]
        [BsonRepresentation((BsonType.ObjectId))]
        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }
                
        public int DurationSeconds { get; private set; }

        public Video(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
