using System.Text.Json;
using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class TicketService : ITicketService
{
    private HttpClient _client;
    private readonly string _ticketUrl = "https://localhost:7229/tickets";
    private readonly string _commentUrl = "https://localhost:7229/comments";
    private readonly string _changelogUrl  = "https://localhost:7229/changelog";

    public TicketService()
    {
        _client = new HttpClient();
    }
    public async Task<List<Ticket>> GetTickets()
    {
        var items = await _client.GetFromJsonAsync<List<Ticket>>(_ticketUrl);
        return items != null ? items.OrderByDescending(t => t.TPriorityID).ToList() : new List<Ticket>();
    }

    public async Task<Ticket?> GetTicketByID(int ticketID)
    {
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{_ticketUrl}/{ticketID}");
        return ticket ?? null;
    }

    public async Task CloseTicket(int ticketId, int userId)
    {
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{_ticketUrl}/{ticketId}");
        if (ticket == null) return;
        var ticketDTO = new TicketDTO
        {
            ID = ticketId,
            TAssignedID = ticket.TAssignedID,
            TCaption = ticket.TCaption,
            TCategoryID = ticket.TCategoryID,
            TClosed = true,
            TCreatedAt = ticket.TCreatedAt,
            TCreatorID = ticket.TCreatorID,
            TDesc = ticket.TDesc,
            TPriorityID = ticket.TPriorityID,
            TRequesterID = ticket.TRequesterID,
            TStatusID = 7,
            TClosedByID = 5 // #TODO HARDCODED USER VALUE
        };
        
        var debugJson = JsonSerializer.Serialize(ticketDTO);
        await _client.PutAsJsonAsync(_ticketUrl, ticketDTO);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket closed", TicketID = ticket.ID, UserID = (int) ticketDTO.TClosedByID
        });
    }

    public async Task EditTicket(Ticket ticket)
    {
        var updatedTicket = new TicketDTO
        {
            ID = ticket.ID,
            TCreatedAt = ticket.TCreatedAt,
            TCaption = ticket.TCaption,
            TAssignedID = ticket.TAssignedID,
            TPriorityID = ticket.TPriorityID,
            TStatusID = ticket.TStatusID,
            TCategoryID = ticket.TCategoryID,
            TDesc = ticket.TDesc,
            TClosed = false,
            TCreatorID = ticket.TCreatorID,
            TRequesterID = ticket.TRequesterID
        };
        await _client.PutAsJsonAsync(_ticketUrl, updatedTicket);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket updated", TicketID = ticket.ID, UserID = ticket.TCreatorID
        });
    }

    public async Task CreateTicket(Ticket ticket)
    {
        var newTicket = new TicketDTO
        {
            TDesc = ticket.TDesc,
            TCaption = ticket.TCaption,
            TCreatorID = ticket.TCreatorID,
            TCategoryID = ticket.TCategoryID,
            TPriorityID = ticket.TPriorityID,
            TStatusID = 1,
            TClosed = false,
            TAssignedID = ticket.TAssignedID,
            TRequesterID = ticket.TRequesterID,
            TCreatedAt = DateTime.Now
        };

        await _client.PostAsJsonAsync(_ticketUrl, newTicket);
    }

    public async Task CreateComment(Comments comment)
    {
        var newComment = new CommentDTO
        {
            Comment = comment.Comment,
            CTime = DateTime.Now,
            TicketID = comment.TicketID,
            UserID = comment.UserID
        };
        await _client.PostAsJsonAsync(_commentUrl, newComment);
    }

    public Task<List<TicketChangelog>> GetLogs(int id)
    {
        var logs = _client.GetFromJsonAsync<List<TicketChangelog>>($"{_changelogUrl}/{id}");
        return logs;
    }

    public async Task CreateChangelog(TicketChangelog tcl)
    {
        var newLog = new TicketChangelogDTO
        {
            EditedAt = DateTime.Now,
            LogText = tcl.LogText,
            TicketID = tcl.TicketID,
            UserID = tcl.UserID
        };
        await _client.PostAsJsonAsync(_changelogUrl, newLog);
    }

    public async Task ReOpenTicket(int ticketId, int userId)
    {       
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{_ticketUrl}/{ticketId}");
        if (ticket == null) return;

        var ticketDTO = new TicketDTO
        {
            ID = ticketId,
            TAssignedID = ticket.TAssignedID,
            TCaption = ticket.TCaption,
            TCategoryID = ticket.TCategoryID,
            TClosed = false,
            TCreatedAt = ticket.TCreatedAt,
            TCreatorID = ticket.TCreatorID,
            TDesc = ticket.TDesc,
            TPriorityID = ticket.TPriorityID,
            TRequesterID = ticket.TRequesterID,
            TStatusID = 2,
            TClosedByID = null
        };
        
        var debugJson = JsonSerializer.Serialize(ticketDTO);
        await _client.PutAsJsonAsync(_ticketUrl, ticketDTO);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket reopened", TicketID = ticket.ID, UserID = userId
        });
    }
}