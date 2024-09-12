using ContactManager_WebApp.Models;
using CsvHelper.Configuration;

namespace ContactManager_WebApp.Mappers
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth).Name("Date of birth");
            Map(m => m.Married).Name("Married");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");
        }
    }
}
