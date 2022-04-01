using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class PropertyService : IPropertyService
{
    private HttpClient _client;
    private readonly string _statusURL = "https://localhost:7229/status";
    private readonly string _priorityURL  = "https://localhost:7229/priority";
    private readonly string _categoryURL = "https://localhost:7229/category";
    private readonly string _userURL = "https://localhost:7229/users";
    private readonly string _roleURL = "https://localhost:7229/roles";

    public PropertyService()
    {
        _client = new HttpClient();
    }

    public async Task<List<Category>> GetCategories()
    {
        var cats = await _client.GetFromJsonAsync<List<Category>>(_categoryURL);
        return cats ?? new List<Category>();
    }

    public async Task<List<Status>> GetStatus()
    {
        var stats = await _client.GetFromJsonAsync<List<Status>>(_statusURL);
        return stats ?? new List<Status>();
    }

    public async Task<List<Priority>> GetPriority()
    {
        var prio = await _client.GetFromJsonAsync<List<Priority>>(_priorityURL);
        return prio ?? new List<Priority>();
    }

    public async Task<List<User>> GetUsers()
    {
        var users = await _client.GetFromJsonAsync<List<User>>(_userURL);
        return users ?? new List<User>();
    }

    public async Task<User> GetUser(int id)
    {
        var users = await GetUsers();
        var user = users.FirstOrDefault(u => u.ID == id);
        return user ?? null;
    }

    public async Task<List<Role>> GetRoles()
    {
        var roles = await _client.GetFromJsonAsync<List<Role>>(_roleURL);
        return roles ?? new List<Role>();
    }

    public async Task CreateCategory(Category category)
    {
        await _client.PostAsJsonAsync(_categoryURL, category);
    }

    public async Task CreateStatus(Status status)
    {
        await _client.PostAsJsonAsync(_statusURL, status);
    }

    public async Task CreatePriority(Priority priority)
    {
        await _client.PostAsJsonAsync(_priorityURL, priority);
    }

    public async Task CreateUser(UserDTO user)
    {
        await _client.PostAsJsonAsync(_userURL, user);
    }

    public async Task UpdateUser(UserDTO user, int ID)
    {
        var editUser = new UserEditDTO
        {
            ID = ID,
            password = user.password,
            display_name = user.display_name,
            email = user.email,
            full_name = user.full_name,
            RoleID = user.RoleID,
            Created_At = user.Created_At
        };
        
        await _client.PutAsJsonAsync(_userURL, editUser);
    }

    public async Task DeleteCategory(int id)
    {
        await _client.DeleteAsync($"{_categoryURL}/{id}");
    }

    public async Task DeleteStatus(int id)
    {
        await _client.DeleteAsync($"{_statusURL}/{id}");
    }

    public async Task DeletePriority(int id)
    {
        await _client.DeleteAsync($"{_priorityURL}/{id}");
}
}