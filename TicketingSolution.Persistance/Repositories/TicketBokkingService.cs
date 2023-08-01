using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Persistance.Repositories
{
    public class TicketBokkingService : ITicketBookingService
    {
        private readonly TicketingSolutionDbContext _context;
        public TicketBokkingService(TicketingSolutionDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<Ticket> GetAcailableTickects(DateTime Date)
        {
            //var UnAvailableTickets = _context.TicketBookings
            //     .Where(t=>t.Date == Date)
            //     .Select(t=>t.TickectID)
            //     .ToList();
            // var AvailableTickets = _context.Tickets
            //     .Where(q => UnAvailableTickets.Contains(q.Id) == false)
            //     .ToList();
            // return AvailableTickets;

            return _context.Tickets
                    .Where(q => q.TicketBookings.Any(x => x.Date == Date))
                    .ToList();


        }

        public void Save(TicketBooking ticketBooking)
        {
            _context.Add(ticketBooking);
            _context.SaveChanges();
        }
    }
}
