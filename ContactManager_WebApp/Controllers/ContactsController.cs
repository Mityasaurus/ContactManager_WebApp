using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager_WebApp.Models;
using ContactManager_WebApp.Services;

namespace ContactManager_WebApp.Controllers
{
    public class ContactsController(ContactManagerContext context, ILogger<ContactsController> logger, ICsvFileReader<Contact> csvFileReader) : Controller
    {
        private readonly ContactManagerContext _context = context;
        private readonly ILogger<ContactsController> _logger = logger;
        private readonly ICsvFileReader<Contact> _csvFileReader = csvFileReader;

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all contacts from the database.");
            var contacts = await _context.Contacts.ToListAsync();
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
                return Json(new { success = false, message = "No file uploaded." });
            }

            try
            {
                _logger.LogInformation("Processing CSV file upload.");
                var records = _csvFileReader.GetRecordsFromFile(file);
                _context.AddRange(records);
                await _context.SaveChangesAsync();
                _logger.LogInformation("CSV file uploaded and records saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing the CSV file.");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] Contact contact)
        {
            if (id != contact.Id)
            {
                _logger.LogWarning("Contact ID mismatch: {Id}", id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for contact update: {ModelState}", ModelState);
                return BadRequest();
            }

            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Contact with ID {Id} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while updating contact with ID {Id}.", id);
                if (!ContactExists(contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Contact ID is null for deletion.");
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found.", id);
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _logger.LogInformation("Contact with ID {Id} deleted successfully.", id);
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent contact with ID {Id}.", id);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
