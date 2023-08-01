using System.ComponentModel.DataAnnotations;
using TicketingSolution.Domain.BaseModels;

namespace TicketingSolution.Domain.Domain
{
    public class TicketBooking: ServiceBookingBase
    {
        public static int Id { get; set; }
        public Ticket Ticket { get; set; } // foreign key
        public int TickectID { get; set; }
    }
}