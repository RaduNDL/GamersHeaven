using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW.Services;

namespace PAW.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetAllAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Email,Message")] Contact contact)
        {
            if (ModelState.IsValid)
                await _contactService.CreateAsync(contact);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackID,Name,Email,Message")] Contact contact)
        {
            var success = await _contactService.UpdateAsync(id, contact);
            return success ? RedirectToAction(nameof(Index)) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
