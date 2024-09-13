using ContactManager_WebApp.Models;

namespace ContactManager_WebApp.Services
{
    public interface ICsvFileReader<T>
    {
        IEnumerable<T> GetRecordsFromFile(IFormFile file);
    }
}
