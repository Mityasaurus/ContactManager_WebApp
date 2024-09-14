using ContactManager_WebApp.Models;

namespace ContactManager_WebApp.Data
{
    public class ContactsRepository(ContactManagerContext context) : Repository<Contact>(context)
    {
        private readonly ContactManagerContext _context = context;
    }
}
