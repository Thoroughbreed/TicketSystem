using System.Net.Http.Headers;
using System.Net.Security;
using Microsoft.AspNetCore.Authentication;
using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class PropertyService : IPropertyService
{
    private HttpClient _client;
    private IHttpContextAccessor _httpContextAccessor;
    
    public PropertyService(IHttpContextAccessor httpContextAccessor, HttpClient client)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        HttpClientHandler clientHandler = new();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => true;
    }
    
    public async Task<string> InitializeHttpClient()
    {
        var BearerToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
        return BearerToken;
    }

    public async Task<List<Category>> GetCategories()
    {
        await InitializeHttpClient();
        var cats = await _client.GetFromJsonAsync<List<Category>>(AppConstants._categoryURL);
        return cats ?? new List<Category>();
    }

    public async Task<List<Status>> GetStatus()
    {
        await InitializeHttpClient();
        var stats = await _client.GetFromJsonAsync<List<Status>>(AppConstants._statusURL);
        return stats ?? new List<Status>();
    }

    public async Task<List<Priority>> GetPriority()
    {
        await InitializeHttpClient();
        var prio = await _client.GetFromJsonAsync<List<Priority>>(AppConstants._priorityURL);
        return prio ?? new List<Priority>();
    }

    public async Task<List<User>> GetUsers()
    {
        await InitializeHttpClient();
        var users = await _client.GetFromJsonAsync<List<User>>(AppConstants._userURL);
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
        await InitializeHttpClient();
        var roles = await _client.GetFromJsonAsync<List<Role>>(AppConstants._roleURL);
        return roles ?? new List<Role>();
    }

    public async Task CreateCategory(Category category)
    {
        await InitializeHttpClient();
        await _client.PostAsJsonAsync(AppConstants._categoryURL, category);
    }

    public async Task CreateStatus(Status status)
    {
        await InitializeHttpClient();
        await _client.PostAsJsonAsync(AppConstants._statusURL, status);
    }

    public async Task CreatePriority(Priority priority)
    {
        await InitializeHttpClient();
        await _client.PostAsJsonAsync(AppConstants._priorityURL, priority);
    }

    public async Task CreateUser(UserDTO user)
    {
        await InitializeHttpClient();
        await _client.PostAsJsonAsync(AppConstants._userURL, user);
    }

    public async Task UpdateUser(UserDTO user, int ID)
    {
        await InitializeHttpClient();
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
        
        await _client.PutAsJsonAsync(AppConstants._userURL, editUser);
    }

    public async Task DeleteCategory(int id)
    {
        await InitializeHttpClient();
        await _client.DeleteAsync($"{AppConstants._categoryURL}/{id}");
    }

    public async Task DeleteStatus(int id)
    {
        await InitializeHttpClient();
        await _client.DeleteAsync($"{AppConstants._statusURL}/{id}");
    }

    public async Task DeletePriority(int id)
    {
        await InitializeHttpClient();
        await _client.DeleteAsync($"{AppConstants._priorityURL}/{id}");
    }
}