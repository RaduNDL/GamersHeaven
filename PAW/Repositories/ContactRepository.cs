using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            return await _context.Contacts.OrderByDescending(c => c.Date).ToListAsync();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
        }

        public Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Contact contact)
        {
            _context.Contacts.Remove(contact);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
