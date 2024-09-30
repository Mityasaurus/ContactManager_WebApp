using ContactManager_WebApp.BusinessLogic.Services;
using ContactManager_WebApp.BusinessLogic.Services.Interfaces;
using ContactManager_WebApp.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManager_WebApp.BusinessLogic.Installers
{
    public static class CsvReaderInstaller
    {
        public static void AddCsvReaderService(this IServiceCollection services)
        {
            services.AddScoped<ICsvFileReader<Contact>, CsvFileReader>();
        }
    }
}
