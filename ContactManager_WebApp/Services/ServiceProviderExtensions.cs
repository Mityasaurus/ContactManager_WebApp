using ContactManager_WebApp.Data;
using ContactManager_WebApp.Data.Interfaces;
using ContactManager_WebApp.Models;

namespace ContactManager_WebApp.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddRepositoryService(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Contact>, ContactsRepository>();
        }

        public static void AddCsvReaderService(this IServiceCollection services)
        {
            services.AddScoped<ICsvFileReader<Contact>, CsvFileReader>();
        }
    }
}
