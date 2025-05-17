using PAW.Models;
using PAW.Repositories;

namespace PAW.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task CreateAsync(Contact contact)
        {
            contact.Date = DateTime.Now;
            await _repository.AddAsync(contact);
            await _repository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, Contact contact)
        {
            if (id != contact.FeedbackID) return false;

            var original = await _repository.GetByIdAsync(id);
            if (original == null) return false;

            original.Name = contact.Name;
            original.Email = contact.Email;
            original.Message = contact.Message;

            await _repository.UpdateAsync(original);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null) return false;

            await _repository.DeleteAsync(contact);
            await _repository.SaveAsync();

            return true;
        }
    }
}
