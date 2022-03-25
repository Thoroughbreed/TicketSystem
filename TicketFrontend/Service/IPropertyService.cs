using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface IPropertyService
{
    public Task<List<Category>> GetCategories();
    public Task<List<Status>> GetStatus();
    public Task<List<Priority>> GetPriority();
    public Task<List<User>> GetUsers();
}