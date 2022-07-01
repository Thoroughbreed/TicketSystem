using Microsoft.EntityFrameworkCore;
using TicketAPI.Models;

namespace TicketAPI.DAL;

public class TicketDb : DbContext
{
    public DbSet<Category> Category { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Priority> Priority { get; set; }
    public DbSet<Requester> Requesters { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<Subscribers> Subscribers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketChangelog> TicketChangelog { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder oB)
    {
        if (!oB.IsConfigured)
        {
            // oB.UseSqlServer("Server = sdktofixdb01; Database = TicketDesk; User Id=program1; Password=smp4519");
            // oB.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TicketTest; Trusted_Connection = True;")
            oB.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TicketDesk; Trusted_Connection = True;")
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(new ServiceCollection()
                .AddLogging(b => b.AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information))
                .BuildServiceProvider()
                .GetService<ILoggerFactory>());
        }
    }

    protected override void OnModelCreating(ModelBuilder mB)
    {
        // User navigation and relations
        mB.Entity<User>()
            .HasOne(b => b.Role);
        mB.Entity<User>()
            .HasMany(u => u.Subscriptions)
            .WithOne()
            .HasForeignKey(k => k.UserID);
        
        
        // Ticket navigation and relations
        mB.Entity<Ticket>()
            .HasOne(t => t.Status)
            .WithMany()
            .HasForeignKey(k => k.TStatusID);
        mB.Entity<Ticket>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(k => k.TCategoryID);
        mB.Entity<Ticket>()
            .HasOne(t => t.Priority)
            .WithMany()
            .HasForeignKey(k => k.TPriorityID);
        // mB.Entity<Ticket>()
        //     .HasOne(t => t.Requester)
        //     .WithMany()
        //     .HasForeignKey(k => k.TRequesterID);
        mB.Entity<Ticket>()
            .HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(k => k.TCreatorID);
        mB.Entity<Ticket>()
            .HasOne(t => t.Asignee)
            .WithMany()
            .HasForeignKey(k => k.TAssignedID);
        mB.Entity<Ticket>()
            .HasOne(t => t.Closer)
            .WithMany()
            .HasForeignKey(k => k.TClosedByID);
        mB.Entity<Ticket>()
            .HasMany(t => t.Comments)
            .WithOne()
            .HasForeignKey(k => k.TicketID);
        mB.Entity<Ticket>()
            .HasMany(t => t.Subscribers)
            .WithOne()
            .HasForeignKey(k => k.TicketID);
    }

    public TicketDb(DbContextOptions<TicketDb> options) : base(options)
    {
    }
}