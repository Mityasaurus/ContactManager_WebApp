using ContactManager_WebApp.DataAccess;
using ContactManager_WebApp.DataAccess.Models;
using ContactManager_WebApp.DataAccess.Repositories;

namespace ContactManager_WebApp.BusinessLogic.Services
{
    public class ContactsRepository(ContactManagerContext context) : Repository<Contact>(context)
    {
        private readonly ContactManagerContext _context = context;
    }
}
