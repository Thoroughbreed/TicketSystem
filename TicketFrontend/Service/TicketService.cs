using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TicketFrontend.DTO;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public class TicketService : ITicketService
{
    private HttpClient _client;
    private IHttpContextAccessor _httpContextAccessor;


    public TicketService(IHttpContextAccessor httpContextAccessor, HttpClient client)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        HttpClientHandler clientHandler = new();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => true;
    }
    
    public async Task<string> InitializeHttpClient()
    {
        var BearerToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        var debug = await _httpContextAccessor.HttpContext.GetTokenAsync("id_token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
        return BearerToken;
    }
    
    public async Task<List<Ticket>> GetTickets()
    {
        await InitializeHttpClient();
        var items = await _client.GetFromJsonAsync<List<Ticket>>(AppConstants._ticketUrl);
        return items != null ? items.OrderByDescending(t => t.TPriorityID).ToList() : new List<Ticket>();
    }

    public async Task<List<Ticket>> GetTicketsQ(int currPage, int pageSize)
    {
        await InitializeHttpClient();
        var items = await _client.GetFromJsonAsync<List<Ticket>>(AppConstants._ticketUrl);
        return items != null ? items.OrderByDescending(t => t.TPriorityID)
            .Skip((currPage - 1) * pageSize)
            .Take(pageSize)
            .ToList() : new List<Ticket>();
    }
    
    public async Task<List<Ticket>> GetTicketsQ(int currPage, int pageSize, TicketOrderOptions options, string search = null)
    {
        var q = await GetTicketsQ(search);
        return q.OrderByOptions(options)
            .Skip((currPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<IQueryable<Ticket>> GetTicketsQ(string? search)
    {
        await InitializeHttpClient();
        var items = await _client.GetFromJsonAsync<List<Ticket>>(AppConstants._ticketUrl);
        int.TryParse(search, out var intSearch);
        return string.IsNullOrWhiteSpace(search) ? items.AsQueryable() : items.Where(t => t.TDesc.Contains(search) || t.TCaption.Contains(search) || t.ID.Equals(intSearch)).AsQueryable();
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        await InitializeHttpClient();
        var tickets = await _client.GetFromJsonAsync<List<Ticket>>($"{AppConstants._ticketUrl}/all");
        return tickets != null ? tickets.Where(t => t.TClosed == true).ToList() : new List<Ticket>();
    }

    public async Task<IQueryable<Ticket>> GetClosedTicketsQ(string? search)
    {
        await InitializeHttpClient();
        var items = await _client.GetFromJsonAsync<List<Ticket>>($"{AppConstants._ticketUrl}/all");

        int.TryParse(search, out var intSearch);
        
        return string.IsNullOrWhiteSpace(search)
            ? items.AsQueryable()
            : items.Where(t => t.TDesc.Contains(search) || t.TCaption.Contains(search) || t.ID.Equals(intSearch))
                .AsQueryable();
    }

    public async Task<List<Ticket>> GetClosedTicketsQ(int currPage, int pageSize, TicketOrderOptions options,
        string search = null)
    {
        var items = await GetClosedTicketsQ(search);
        return items != null ? items.Where(t => t.TClosed)
            .OrderByOptions(options)
            .Skip((currPage - 1) * pageSize)
            .Take(pageSize)
            .ToList() : new List<Ticket>();
    }

    public async Task<List<Ticket>> GetClosedTicketsQ(int currPage, int pageSize)
    {
        await InitializeHttpClient();
        var items = await _client.GetFromJsonAsync<List<Ticket>>($"{AppConstants._ticketUrl}/all");
        return items != null ? items
            .Where(t => t.TClosed == true)
            .OrderByDescending(t => t.TPriorityID)
            .Skip((currPage - 1) * pageSize)
            .Take(pageSize)
            .ToList() : new List<Ticket>();
    }

    public async Task<Ticket?> GetTicketByID(int ticketID)
    {
        await InitializeHttpClient();
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{AppConstants._ticketUrl}/{ticketID}");
        return ticket ?? null;
    }

    public async Task CloseTicket(int ticketId, int userId)
    {
        await InitializeHttpClient();
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{AppConstants._ticketUrl}/{ticketId}");
        if (ticket == null) return;
        var ticketDTO = new TicketDTO
        {
            ID = ticketId,
            // TAssignedID = ticket.TAssignedID,
            TCaption = ticket.TCaption,
            TCategoryID = ticket.TCategoryID,
            TClosed = true,
            TCreatedAt = ticket.TCreatedAt,
            TClosedAt = DateTime.Now,
            TCreatorID = ticket.TCreatorID,
            TDesc = ticket.TDesc,
            TPriorityID = ticket.TPriorityID,
            // TRequesterID = ticket.TRequesterID,
            TStatusID = 8, // 8 is hardcoded, statusID for closed ticket
            TClosedByID = userId
        };
        
        var debugJson = JsonSerializer.Serialize(ticketDTO);
        await _client.PutAsJsonAsync(AppConstants._ticketUrl, ticketDTO);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket closed", TicketID = ticket.ID, UserID = (int) ticketDTO.TClosedByID
        });
    }

    public async Task EditTicket(Ticket ticket)
    {
        await InitializeHttpClient();
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
            // TRequesterID = ticket.TRequesterID
        };
        await _client.PutAsJsonAsync(AppConstants._ticketUrl, updatedTicket);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket updated", TicketID = ticket.ID, UserID = ticket.TCreatorID
        });
    }

    public async Task<string> CreateTicket(TicketDTO ticket)
    {
        await InitializeHttpClient();
        var newID = await _client.PostAsJsonAsync(AppConstants._ticketUrl, ticket);
        var returnValue = await newID.Content.ReadAsStringAsync();
        return returnValue;
    }

    public async Task CreateComment(Comments comment)
    {
        await InitializeHttpClient();
        var newComment = new CommentDTO
        {
            Comment = comment.Comment,
            CTime = DateTime.Now,
            TicketID = comment.TicketID,
            UserID = comment.UserID
        };
        await _client.PostAsJsonAsync(AppConstants._commentUrl, newComment);
    }

    public Task<List<TicketChangelog>> GetLogs(int id)
    {
        InitializeHttpClient().WaitAsync(CancellationToken.None);
        var logs = _client.GetFromJsonAsync<List<TicketChangelog>>($"{AppConstants._changelogUrl}/{id}");
        return logs;
    }

    public async Task CreateChangelog(TicketChangelog tcl)
    {
        await InitializeHttpClient();
        var newLog = new TicketChangelogDTO
        {
            EditedAt = DateTime.Now,
            LogText = tcl.LogText,
            TicketID = tcl.TicketID,
            UserID = tcl.UserID
        };
        await _client.PostAsJsonAsync(AppConstants._changelogUrl, newLog);
    }

    public async Task ReOpenTicket(int ticketId, int userId)
    {       
        await InitializeHttpClient();
        var ticket = await _client.GetFromJsonAsync<Ticket>($"{AppConstants._ticketUrl}/{ticketId}");
        if (ticket == null) return;

        var ticketDTO = new TicketDTO
        {
            ID = ticketId,
            // TAssignedID = ticket.TAssignedID,
            TCaption = ticket.TCaption,
            TCategoryID = ticket.TCategoryID,
            TClosed = false,
            TCreatedAt = ticket.TCreatedAt,
            TCreatorID = ticket.TCreatorID,
            TDesc = ticket.TDesc,
            TPriorityID = ticket.TPriorityID,
            // TRequesterID = ticket.TRequesterID,
            TStatusID = userId,
            TClosedByID = null,
            TClosedAt = null
        };
        
        var debugJson = JsonSerializer.Serialize(ticketDTO);
        await _client.PutAsJsonAsync(AppConstants._ticketUrl, ticketDTO);
        await CreateChangelog(new TicketChangelog
        {
            LogText = "Ticket reopened", TicketID = ticket.ID, UserID = userId
        });
    }
}