using Microsoft.AspNetCore.Http;

namespace ContactManager_WebApp.BusinessLogic.Services.Interfaces
{
    public interface ICsvFileReader<T>
    {
        IEnumerable<T> GetRecordsFromFile(IFormFile file);
    }
}
