using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class TicketService : ITicketService
{
    private HttpClient _client;

    public TicketService()
    {
        _client = new();
    }
    public async Task<List<Ticket>> GetTickets()
    {
        var items = await _client.GetFromJsonAsync<List<Ticket>>("https://localhost:7229/tickets");
        return items != null ? items.OrderByDescending(t => t.TPriorityID).ToList() : new List<Ticket>();
    }
}