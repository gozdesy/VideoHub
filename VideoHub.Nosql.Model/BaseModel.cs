using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VideoHub.Nosql.Model
{
    //[BsonIgnoreExtraElements]
    public class BaseModel
    {       
        //[BsonExtraElements]
        //public BsonDocument CatchAll { get; set; }
    }
}
