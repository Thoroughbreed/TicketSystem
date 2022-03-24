﻿using System.Text.Json;
using TicketFrontend.DTO;
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
}