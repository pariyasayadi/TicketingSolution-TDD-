using TicketingSolution.Core.Enums;
using TicketingSolution.Domain.BaseModels;

namespace TicketingSolution.Core.Model
{
    public class ServiceBookingResult : ServiceBookingBase
    {
        public BookingResultFlag Flag { get; set; }
        public object TicketBookingId { get; set; }
    }
}