namespace ContactManager_WebApp.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<bool> AddAsync(T data);

        Task<bool> UpdateAsync(T data);

        Task<bool> AddRangeAsync(IEnumerable<T> data);
    }
}
