using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager_WebApp.Models;
using ContactManager_WebApp.Services;
using ContactManager_WebApp.Data.Interfaces;

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
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return View("_Error", new ErrorViewModel { Message = "No file was uploaded." });
            }

            _logger.LogInformation("Processing CSV file upload.");
            IEnumerable<Contact> records;
            try
            {
                records = _csvFileReader.GetRecordsFromFile(file);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("_Error", new ErrorViewModel { Message = "Invalid file format." });
            }

            var result = await _repository.AddRangeAsync(records);
            if (result)
            {
                _logger.LogInformation("CSV file uploaded and records saved successfully.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                string message = "Error occurred while saving records to the database.";
                _logger.LogError(message);
                return View("_Error", new ErrorViewModel { Message = message });
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
                return View("_Error", new ErrorViewModel { Message = "Contact ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for contact update.");
                return View("_Error", new ErrorViewModel { Message = "Invalid data provided." });
            }

            var result = await _repository.UpdateAsync(contact);
            if (result)
            {
                _logger.LogInformation("Contact with ID {Id} updated successfully.", id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Error occurred while updating contact with ID {Id}.", id);
                if (!ContactExists(contact.Id))
                {
                    return View("_Error", new ErrorViewModel { Message = "Contact not found." });
                }
                else
                {
                    return View("_Error", new ErrorViewModel { Message = "An error occurred while updating the contact." });
                }
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
                    _logger.LogError("Error occurred while deleting contact with ID {Id}.", id);
                    return View("_Error", new ErrorViewModel { Message = "An error occurred while deleting the contact." });
                }
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent contact with ID {Id}.", id);
                return View("_Error", new ErrorViewModel { Message = "Contact not found." });
            }
        }

        private bool ContactExists(int id)
        {
            return _repository.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}
