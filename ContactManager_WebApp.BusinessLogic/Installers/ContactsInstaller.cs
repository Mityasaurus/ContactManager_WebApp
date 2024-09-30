using ContactManager_WebApp.BusinessLogic.Services;
using ContactManager_WebApp.DataAccess.Models;
using ContactManager_WebApp.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManager_WebApp.BusinessLogic.Installers
{
    public static class ContactsInstaller
    {
        public static void AddContactsRepository(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Contact>, ContactsRepository>();
        }
    }
}
