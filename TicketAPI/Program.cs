using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TicketAPI.DAL;
using TicketAPI.Models;
using static Microsoft.AspNetCore.Http.Results;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
//builder.Services.AddDbContext<TicketDb>(option => option.UseInMemoryDatabase("TicketList"));
builder.Services.AddDbContext<TicketDb>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");


app.MapGet("/tickets", async (TicketDb db) =>
{
    var tickets = await db.Tickets
        .Where(t => t.TClosed == false)
        .Include(t => t.Priority)
        .Include(t => t.Status)
        .Include(t => t.Category)
        .Include(t => t.Creator)
        .Include(t => t.Asignee)
        .Include(t => t.Requester)
        .Include(t => t.Comments)
            .ThenInclude(c => c.User)
        .ToListAsync();
    return tickets;
});

app.MapPut("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Update(ticket);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
});

app.MapPost("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Add(ticket);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});



app.MapPost("/Comments", async (Comments comment, TicketDb db) =>
{
    db.Add(comment);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});



app.MapGet("/users", async (TicketDb db) =>
{
    var users = await db.Users
        .Include(u => u.Role)
        .ToListAsync();
    return users;
});


app.Run();