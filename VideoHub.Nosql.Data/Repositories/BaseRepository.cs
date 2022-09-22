using MongoDB.Driver;
using VideoHub.Nosql.Model;

namespace VideoHub.Nosql.Data
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        protected IMongoDatabase Database { get; private set; }
        protected string CollectionName { get; private set; }

        public BaseRepository(IVideoDbContext context, string collectionName)
        {
            Database = context.Database;
            CollectionName = collectionName;
        }

        public async Task<IEnumerable<T>> GetItems()
        {
            var items = await Database.GetCollection<T>(CollectionName).FindAsync(s => true);
            return items.ToEnumerable();
        }
    }
}
