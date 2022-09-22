using VideoHub.Nosql.Model;

namespace VideoHub.Nosql.Data
{
    public class VideoRepository : BaseRepository<Video>, IVideoRepository
    {
        public VideoRepository(IVideoDbContext context) : base(context, "Video")
        {
            
        }
    }
}
