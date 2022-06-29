using System.ComponentModel.DataAnnotations;
using TicketFrontend.Models;

namespace TicketFrontend.Service;

public enum TicketOrderOptions
{
    [Display(Name = "Prioritet")]
    ByPrio = 0,
    [Display(Name = "Prioritet faldende")]
    ByPrioDesc,
    [Display(Name = "Ticket ID")]
    ByID,
    [Display(Name = "Ticket ID faldende")]
    ByIdDesc,
    [Display(Name = "Status")]
    ByStatus,
    [Display(Name = "Status faldende")]
    ByStatusDesc
}

public static class OrderOptions
{
    public static IQueryable<Ticket> OrderByOptions(this IQueryable<Ticket> products, TicketOrderOptions orderOptions)
    {
        return orderOptions switch
        {
            TicketOrderOptions.ByID => products.OrderBy(x => x.ID),
            TicketOrderOptions.ByPrio => products.OrderByDescending(x => x.TPriorityID),
            TicketOrderOptions.ByIdDesc => products.OrderByDescending(x => x.ID),
            TicketOrderOptions.ByPrioDesc => products.OrderBy(x => x.TPriorityID),
            TicketOrderOptions.ByStatus => products.OrderBy(x => x.TStatusID),
            TicketOrderOptions.ByStatusDesc => products.OrderByDescending(x => x.TStatusID),
            _ => throw new ArgumentOutOfRangeException(nameof(orderOptions), orderOptions, null)
        };
    }
}