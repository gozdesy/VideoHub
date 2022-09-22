using MongoDB.Driver;

namespace VideoHub.Nosql.Data
{
    public interface IVideoDbContext 
    {
        IMongoDatabase Database { get; }
    }

    public class VideoDbContext : IVideoDbContext
    {
        public IMongoDatabase Database { get; private set; }

        public VideoDbContext(IMongoDatabase database)
        {
            Database = database;
        }
    }
}