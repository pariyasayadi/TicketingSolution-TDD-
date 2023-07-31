using System;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;

namespace TicketingSolution.Core.Test
{
    public class TicketBookingRequestHandller
    {
        private readonly ITicketBookingService _ticketBookingService;

        public TicketBookingRequestHandller(ITicketBookingService ticketBookingService)
        {
            this._ticketBookingService = ticketBookingService;
        }

        public ServiceBookingResult BookService(TicketBookingRequest bookingReqest)
        {
            if (bookingReqest is null)
            {
                throw new ArgumentNullException(nameof(bookingReqest));
            }
            var availableTicket = _ticketBookingService.GetAcailableTickects(bookingReqest.Date);
            if (availableTicket.Any())
            {
                var Ticket = availableTicket.First();
                var TickectBooking = CreateTicketBookingObject<TicketBooking>(bookingReqest);
                TickectBooking.TickectID = Ticket.Id;
                _ticketBookingService.Save(TickectBooking);
            }

            return CreateTicketBookingObject<ServiceBookingResult>(bookingReqest);
        }
        private static ITicketBooking CreateTicketBookingObject<ITicketBooking>(TicketBookingRequest bookingReqest) where ITicketBooking 
            : ServiceBookingBase, new()
        {
            return new ITicketBooking
            {
                Name = bookingReqest.Name,
                Family = bookingReqest.Family,
                Email = bookingReqest.Email,
            };
       }
    }
}