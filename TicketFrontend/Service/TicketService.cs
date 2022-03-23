using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class TicketService : ITicketService
{
    private HttpClient _client;
    private readonly string _ticketUrl = "https://localhost:7229/tickets";
    private readonly string _commentUrl = "https://localhost:7229/comments";

    public TicketService()
    {
        _client = new();
    }
    public async Task<List<Ticket>> GetTickets()
    {
        var items = await _client.GetFromJsonAsync<List<Ticket>>(_ticketUrl);
        return items != null ? items.OrderByDescending(t => t.TPriorityID).ToList() : new List<Ticket>();
    }

    public async Task CloseTicket(int ticketId, int userId)
    {
        var tickets = await _client.GetFromJsonAsync<List<Ticket>>(_ticketUrl);
        var ticket = tickets.FirstOrDefault(t => t.ID == ticketId);
        ticket.TClosedByID = userId;
        ticket.TClosed = true;

        await _client.PutAsJsonAsync(_ticketUrl, ticket);
    }

    public async Task EditTicket(Ticket ticket)
    {
        await _client.PutAsJsonAsync(_ticketUrl, ticket);
    }

    public async Task CreateComment(Comments comment)
    {
        await _client.PostAsJsonAsync(_commentUrl, comment);
    }
}