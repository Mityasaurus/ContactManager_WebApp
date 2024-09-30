using ContactManager_WebApp.BusinessLogic.Services.Interfaces;
using ContactManager_WebApp.DataAccess.Models;
using ContactManager_WebApp.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager_WebApp.Controllers
{
    public class ContactsController(IRepository<Contact> repository, ILogger<ContactsController> logger, ICsvFileReader<Contact> csvFileReader) : Controller
    {
        private readonly IRepository<Contact> _repository = repository;
        private readonly ILogger<ContactsController> _logger = logger;
        private readonly ICsvFileReader<Contact> _csvFileReader = csvFileReader;

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all contacts from the database.");
            var contacts = await _repository.GetAllAsync();
            return View(contacts);
        }

        // GET: Contacts/UploadCsv
        public IActionResult UploadCsv()
        {
            _logger.LogInformation("Displaying CSV upload page.");
            return View();
        }

        // POST: Contacts/UploadCsv
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file?.Length <= 0 || file == null)
            {
                _logger.LogWarning("Invalid or empty file uploaded.");
                return ErrorView("No file was uploaded or file is empty.");
            }

            _logger.LogInformation("Processing CSV file upload.");
            IEnumerable<Contact> records;
            try
            {
                records = _csvFileReader.GetRecordsFromFile(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing CSV file.");
                return ErrorView("Invalid file format.");
            }

            var result = await _repository.AddRangeAsync(records);
            if (result)
            {
                _logger.LogInformation("CSV file uploaded and records saved successfully.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return ErrorView("Error occurred while saving records to the database.");
            }
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Contact contact)
        {
            if (id != contact.Id)
            {
                _logger.LogWarning("Contact ID mismatch: {Id}", id);
                return ErrorView("Contact ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state.");
                return ErrorView("Invalid data provided.");
            }

            var result = await _repository.UpdateAsync(contact);
            if (result)
            {
                _logger.LogInformation("Contact with ID {Id} updated successfully.", id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return ErrorView("An error occurred while updating the contact.");
            }
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Contact ID is null for deletion.");
                return View("_Error", new ErrorViewModel { Message = "Contact ID is required." });
            }

            var contact = await _repository.GetAsync(id.Value);
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found.", id);
                return View("_Error", new ErrorViewModel { Message = "Contact not found." });
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _repository.GetAsync(id);
            if (contact != null)
            {
                var result = await _repository.DeleteAsync(id);
                if (result)
                {
                    _logger.LogInformation("Contact with ID {Id} deleted successfully.", id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return ErrorView("An error occurred while deleting the contact.");
                }
            }
            else
            {
                return ErrorView("Contact not found.");
            }
        }

        private bool ContactExists(int id)
        {
            return _repository.GetAllAsync().Result.Any(e => e.Id == id);
        }

        private ViewResult ErrorView(string message)
        {
            return View("_Error", new ErrorViewModel { Message = message });
        }
    }
}
