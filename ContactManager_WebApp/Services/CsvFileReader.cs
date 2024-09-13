using ContactManager_WebApp.Mappers;
using ContactManager_WebApp.Models;
using CsvHelper;
using System.Text;

namespace ContactManager_WebApp.Services
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
