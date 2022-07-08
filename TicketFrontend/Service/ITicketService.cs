using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public interface ITicketService
{
    public Task<List<Ticket>> GetTickets();
    public Task<IQueryable<Ticket>> GetTicketsQ(string? search);
    public Task<List<Ticket>> GetTicketsQ(int currPage, int pageSize);
    public Task<List<Ticket>> GetTicketsQ(int currPage, int pageSize, TicketOrderOptions options, string search = null);

    public Task<List<Ticket>> GetTicketsQ(int currPage, int pageSize, TicketOrderOptions options, bool onlyMine, int uID, string search = null);
    public Task<List<Ticket>> GetAllTickets();
    public Task<List<Ticket>> GetClosedTicketsQ(int currPage, int pageSize);

    public Task<List<Ticket>> GetClosedTicketsQ(int currPage, int pageSize, TicketOrderOptions options,
        string search = null);
    public Task<Ticket?> GetTicketByID(int ticketID);
    public Task CloseTicket(int ticketId, int userId);
    public Task ReOpenTicket(int ticketId, int userId);
    public Task EditTicket(Ticket ticket);
    public Task<string> CreateTicket(TicketDTO ticket);
    public Task CreateComment(Comments comment);
    public Task<List<TicketChangelog>> GetLogs(int ticketId);
    public Task CreateChangelog(TicketChangelog tcl);

}