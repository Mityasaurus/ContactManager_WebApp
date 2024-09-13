using ContactManager_WebApp.Models;

namespace ContactManager_WebApp.Services
{
    public class ContactsRepository(ContactManagerContext context) : Repository<Contact>(context)
    {
        private readonly ContactManagerContext _context = context;
    }
}
