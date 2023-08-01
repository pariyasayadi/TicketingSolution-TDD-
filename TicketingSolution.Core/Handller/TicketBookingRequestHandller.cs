using System;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Model;
using TicketingSolution.Domain.BaseModels;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Core.Handller
{
    public class TicketBookingRequestHandller
    {
        private readonly ITicketBookingService _ticketBookingService;

        public TicketBookingRequestHandller(ITicketBookingService ticketBookingService)
        {
            _ticketBookingService = ticketBookingService;
        }

        public ServiceBookingResult BookService(TicketBookingRequest bookingReqest)
        {
            if (bookingReqest is null)
            {
                throw new ArgumentNullException(nameof(bookingReqest));
            }
            var availableTicket = _ticketBookingService.GetAcailableTickects(bookingReqest.Date);
            var result = CreateTicketBookingObject<ServiceBookingResult>(bookingReqest);
            if (availableTicket.Any())
            {
                var Ticket = availableTicket.First();
                var TickectBooking = CreateTicketBookingObject<TicketBooking>(bookingReqest);
                TickectBooking.TickectID = Ticket.Id;
                _ticketBookingService.Save(TickectBooking);
                result.Flag = Enums.BookingResultFlag.Success;
                result.TicketBookingId = TickectBooking.TickectID;
            }
            else
            {
                result.Flag = Enums.BookingResultFlag.Failure;
            }

            return result;
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