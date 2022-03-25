using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class PropertyService : IPropertyService
{
    private HttpClient _client;
    private readonly string _statusURL = "https://localhost:7229/status";
    private readonly string _priorityURL  = "https://localhost:7229/priority";
    private readonly string _categoryURL = "https://localhost:7229/category";

    public PropertyService()
    {
        _client = new HttpClient();
    }

    public async Task<List<Category>> GetCategories()
    {
        var cats = await _client.GetFromJsonAsync<List<Category>>(_categoryURL);
        return cats;
    }

    public async Task<List<Status>> GetStatus()
    {
        var stats = await _client.GetFromJsonAsync<List<Status>>(_statusURL);
        return stats;
    }

    public async Task<List<Priority>> GetPriority()
    {
        var prio = await _client.GetFromJsonAsync<List<Priority>>(_priorityURL);
        return prio;
    }
}