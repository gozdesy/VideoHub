namespace VideoHub.Nosql.Data
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetItems();
    }
}
