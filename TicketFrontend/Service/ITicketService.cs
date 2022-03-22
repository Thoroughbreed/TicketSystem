using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface ITicketService
{
    public Task<List<Ticket>> GetTickets();
}