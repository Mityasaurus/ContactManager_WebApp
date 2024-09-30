using ContactManager_WebApp.BusinessLogic.Mappers;
using ContactManager_WebApp.BusinessLogic.Services.Interfaces;
using ContactManager_WebApp.DataAccess.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ContactManager_WebApp.BusinessLogic.Services
{
    public class CsvFileReader : ICsvFileReader<Contact>
    {
        public IEnumerable<Contact> GetRecordsFromFile(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            using var csv = new CsvReader(stream, System.Globalization.CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<ContactMap>();
            var records = csv.GetRecords<Contact>().ToList();
            return records;
        }
    }
}
