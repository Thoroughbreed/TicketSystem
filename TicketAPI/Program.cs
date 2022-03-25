using System.Formats.Asn1;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TicketAPI.DAL;
using TicketAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<TicketDb>();

// Uncomment lines below to use authentication roles for API
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("R", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "ticket:read"));
//     options.AddPolicy("E", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "ticket:write"));
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Uncomment line below to use authentication for API
// app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

#region TICKETS
// Gets a list of all tickets not closed
app.MapGet("/tickets", async (TicketDb db) =>
{
    var tickets = await db.Tickets
        .Where(t => t.TClosed == false)
        .Include(t => t.Priority)
        .Include(t => t.Status)
        .Include(t => t.Category)
        .Include(t => t.Asignee)
        .Include(t => t.Requester)
        .AsNoTracking()
        .ToListAsync();
    return tickets;
});

// Gets a list of *all* tickets
app.MapGet("/tickets/all", async (TicketDb db) =>
{
    var tickets = await db.Tickets
        .Include(t => t.Priority)
        .Include(t => t.Status)
        .Include(t => t.Category)
        .Include(t => t.Asignee)
        .Include(t => t.Requester)
        .AsNoTracking()
        .ToListAsync();
    return tickets;
});

// Gets a single ticket from ID including all sub tables
app.MapGet("/tickets/{id}", async (int id, TicketDb db) =>
{
    var ticket = await db.Tickets.Where(t => t.ID == id)
        .Include(t => t.Priority)
        .Include(t => t.Closer)
        .Include(t => t.Status)
        .Include(t => t.Category)
        .Include(t => t.Creator)
        .Include(t => t.Asignee)
        .Include(t => t.Requester)
        .Include(t => t.Comments)
        .ThenInclude(c => c.User)
        .AsNoTracking()
        .FirstOrDefaultAsync();
    return ticket ?? null;
});

// Updates a ticket
app.MapPut("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Update(ticket);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
});

// Creates a new ticket
app.MapPost("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Add(ticket);
    await db.SaveChangesAsync();
    var newID = await db.Tickets.OrderByDescending(t => t.ID).FirstOrDefaultAsync();
    return newID?.ID ?? 0;
});

    #region COMMENTS
    // Inserts a comment in a ticket, no need to have anything else than post
    app.MapPost("/Comments", async (Comments comment, TicketDb db) =>
        {
            db.Add(comment);
            await db.SaveChangesAsync();
            return HttpStatusCode.Created;
        });
    #endregion

    #region Changelog
    // Updates the ticket changelog
    app.MapGet("/Changelog/{id}", async (int id, TicketDb db) =>
    {
        var logs = await db.TicketChangelog
            .Where(t => t.TicketID == id)
            .Include(t => t.User)
            .AsNoTracking()
            .ToListAsync();
        return logs;
    });
    
    app.MapPost("/Changelog", async (TicketChangelog tcl, TicketDb db) =>
    {
        db.Add(tcl);
        await db.SaveChangesAsync();
        return HttpStatusCode.Created;
    });
    #endregion

#endregion


#region USERS
// Lists all users
    app.MapGet("/users", async (TicketDb db) =>
    {
        var users = await db.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .ToListAsync();
        return users;
    });

// Updates a current user
    app.MapPut("/users", async (User user, TicketDb db) =>
    {
        db.Update(user);
        await db.SaveChangesAsync();
        return HttpStatusCode.OK;
    });

// Creates a new user
    app.MapPost("/users", async (User user, TicketDb db) =>
    {
        db.Add(user);
        await db.SaveChangesAsync();
        return HttpStatusCode.OK;
    });
#endregion

#region SUB PROPERTIES
app.MapGet("/roles", async (TicketDb db) =>
    await db.Roles.ToListAsync());

app.MapPost("/roles", async (Role role, TicketDb db) =>
{
    db.Roles.Add(role);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});


app.MapGet("/status", async (TicketDb db) =>
    await db.Status.ToListAsync());

app.MapPost("/status", async (Status status, TicketDb db) =>
{
    db.Status.Add(status);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});


app.MapGet("/priority", async (TicketDb db) =>
    await db.Priority.ToListAsync());

app.MapPost("/priority", async (Priority priority, TicketDb db) =>
{
    db.Priority.Add(priority);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});


app.MapGet("/category", async (TicketDb db) =>
    await db.Category.ToListAsync());

app.MapPost("/category", async (Category category, TicketDb db) =>
{
    db.Category.Add(category);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
});
#endregion

app.Run();