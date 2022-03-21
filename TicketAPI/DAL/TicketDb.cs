using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL;

public class TicketDb : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder oB)
    {
        if (!oB.IsConfigured)
        {
            oB.UseSqlServer("Server = sdktofixdb01; Database = TicketDesk; User Id=program1; Password=smp4519");
        }
    }
    
    public TicketDb(DbContextOptions<TicketDb> options) : base(options)
    {
    }
    
//    public DbSet<Ticket> Tickets => Set<Ticket>();
}