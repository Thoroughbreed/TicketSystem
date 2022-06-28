using System.Net;
using System.Security.Claims;
using System.IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketAPI.DAL;
using TicketAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<TicketDb>();
string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("T", policy => policy.RequireRole("Sudo"));
    // // options.AddPolicy("TEST2", policy => policy.RequireAuthenticatedUser().RequireRole("Sudo"));
    // options.AddPolicy("R", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "ticket:read"));
    // options.AddPolicy("W", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "ticket:write"));
    // options.AddPolicy("A", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "ticket:admin"));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o =>
{
    o.AllowAnyOrigin();
    o.AllowAnyHeader();
    o.AllowAnyMethod();
});

app.UseHttpsRedirection();

// Uncomment line below to use authentication for API
app.UseAuthentication();
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
}).RequireAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();

// Updates a ticket
app.MapPut("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Update(ticket);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
}).RequireAuthorization();

// Creates a new ticket
app.MapPost("/tickets", async (Ticket ticket, TicketDb db) =>
{
    db.Add(ticket);
    await db.SaveChangesAsync();
    var newID = await db.Tickets.OrderByDescending(t => t.ID).FirstOrDefaultAsync();
    return newID?.ID ?? 0;
}).RequireAuthorization();

    #region COMMENTS
    // Inserts a comment in a ticket, no need to have anything else than post
    app.MapPost("/Comments", async (Comments comment, TicketDb db) =>
        {
            db.Add(comment);
            await db.SaveChangesAsync();
            return HttpStatusCode.Created;
        }).RequireAuthorization();
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
    }).RequireAuthorization();
    
    app.MapPost("/Changelog", async (TicketChangelog tcl, TicketDb db) =>
    {
        db.Add(tcl);
        await db.SaveChangesAsync();
        return HttpStatusCode.Created;
    }).RequireAuthorization();
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
    }).RequireAuthorization();

// Updates a current user
    app.MapPut("/users", async (User user, TicketDb db) =>
    {
        db.Update(user);
        await db.SaveChangesAsync();
        return HttpStatusCode.OK;
    }).RequireAuthorization();

// Creates a new user
    app.MapPost("/users", async (User user, TicketDb db) =>
    {
        db.Add(user);
        await db.SaveChangesAsync();
        return HttpStatusCode.OK;
    }).RequireAuthorization();
#endregion

#region SUB PROPERTIES
app.MapGet("/roles", async (TicketDb db) =>
    await db.Roles.ToListAsync()).RequireAuthorization();

app.MapPost("/roles", async (Role role, TicketDb db) =>
{
    db.Roles.Add(role);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
}).RequireAuthorization();

app.MapPut("/roles/{id}", async (int id, TicketDb db) =>
{
    var found = db.Roles.FirstOrDefault(e => e.ID == id);
    db.Roles.Remove(found);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
}).RequireAuthorization();


app.MapGet("/status", async (TicketDb db) =>
    await db.Status.ToListAsync()).RequireAuthorization();

app.MapPost("/status", async (Status status, TicketDb db) =>
{
    db.Status.Add(status);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
}).RequireAuthorization();

app.MapDelete("/status/{id}", async (int id, TicketDb db) =>
{
    var found = db.Status.FirstOrDefault(e => e.ID == id);
    db.Status.Remove(found);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
}).RequireAuthorization();


app.MapGet("/priority", async (TicketDb db) =>
    await db.Priority.ToListAsync()).RequireAuthorization();

app.MapPost("/priority", async (Priority priority, TicketDb db) =>
{
    db.Priority.Add(priority);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
}).RequireAuthorization();

app.MapDelete("/priority/{id}", async (int id, TicketDb db) =>
{
    var found = db.Priority.FirstOrDefault(e => e.ID == id);
    db.Priority.Remove(found);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
}).RequireAuthorization();


app.MapGet("/category", async (TicketDb db) =>
    await db.Category.ToListAsync()).RequireAuthorization();

app.MapPost("/category", async (Category category, TicketDb db) =>
{
    db.Category.Add(category);
    await db.SaveChangesAsync();
    return HttpStatusCode.Created;
}).RequireAuthorization();

app.MapDelete("/category/{id}", async (int id, TicketDb db) =>
{
    var found = db.Category.FirstOrDefault(e => e.ID == id);
    db.Category.Remove(found);
    await db.SaveChangesAsync();
    return HttpStatusCode.OK;
}).RequireAuthorization();

#endregion

app.Run();

public abstract class HasScopeRequirement : IAuthorizationRequirement
{
    public string Issuer { get; }
    public string Scope { get; }

    protected HasScopeRequirement(string scope, string issuer)
    {
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
    }
}

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        // If user does not have the scope claim, get out of here
        if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
            return Task.CompletedTask;

        // Split the scopes string into an array
        var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

        // Succeed if the scope array contains the required scope
        if (scopes.Any(s => s == requirement.Scope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}