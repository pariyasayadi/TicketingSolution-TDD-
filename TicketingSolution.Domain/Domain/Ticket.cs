using System.ComponentModel.DataAnnotations;

namespace TicketingSolution.Domain.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TicketBooking> TicketBookings { get; set; }
    }
}