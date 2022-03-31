using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface IPropertyService
{
    public Task<List<Category>> GetCategories();
    public Task<List<Status>> GetStatus();
    public Task<List<Priority>> GetPriority();
    public Task<List<User>> GetUsers();
    public Task<User> GetUser(int id);
    public Task<List<Role>> GetRoles();
    public Task CreateCategory(Category category);
    public Task CreateStatus(Status status);
    public Task CreatePriority(Priority priority);
    public Task CreateUser(UserDTO user);
    public Task DeleteCategory(int id);
    public Task DeleteStatus(int id);
    public Task DeletePriority(int id);
}