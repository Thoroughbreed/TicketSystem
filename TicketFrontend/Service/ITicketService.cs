using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface ITicketService
{
    public Task<List<Ticket>> GetTickets();
    public Task<Ticket?> GetTicketByID(int ticketID);
    public Task CloseTicket(int ticketId, int userId);
    public Task ReOpenTicket(int ticketId, int userId);
    public Task EditTicket(Ticket ticket);
    public Task CreateTicket(Ticket ticket);
    public Task CreateComment(Comments comment);
    public Task<List<TicketChangelog>> GetLogs(int ticketId);
    public Task CreateChangelog(TicketChangelog tcl);

}