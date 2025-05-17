using PAW.Models;

namespace PAW.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllAsync();
        Task CreateAsync(Contact contact);
        Task<bool> UpdateAsync(int id, Contact contact);
        Task<bool> DeleteAsync(int id);
    }
}
