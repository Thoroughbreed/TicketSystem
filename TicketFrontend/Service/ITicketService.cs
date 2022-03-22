using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface ITicketService
{
    public Task<List<Ticket>> GetTickets();
    public Task CloseTicket(int ticketId, int userId);
    public Task EditTicket(Ticket ticket);
    public Task CreateComment(Comments comment);

}