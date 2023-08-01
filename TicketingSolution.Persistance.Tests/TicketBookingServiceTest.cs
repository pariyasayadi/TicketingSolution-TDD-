using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketingSolution.Domain.Domain;
using TicketingSolution.Persistance.Repositories;
using Xunit;

namespace TicketingSolution.Persistance.Tests
{
    public class TicketBookingServiceTest
    {

        [Fact]
        public void ShouldReturnAvailabeServices()
        {
            //Arrange
            var date = new DateTime(2023, 08, 01);
            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
                .UseInMemoryDatabase("AvailableTickeTest", b => b.EnableNullChecks(false))
                .Options;

            using var context = new TicketingSolutionDbContext(dbOptions);
            context.Add(new Ticket { Id = 1, Name = "Ticket 1" });
            context.Add(new Ticket { Id = 2, Name = "Ticket 2" });
            context.Add(new Ticket { Id = 3, Name = "Ticket 3" });

            //context.Add(new TicketBooking { TickectID = 1, Name = "T1", Family = "TF1", Email = "T1@t1.com", Date = date });
            //context.Add(new TicketBooking { TickectID = 2, Name = "T2", Family = "TF2", Email = "T1@t2.com", Date = date.AddDays(-1) });

            context.Add(new TicketBooking { TickectID = 1, Date = date });
            context.Add(new TicketBooking { TickectID = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var ticketBookingService = new TicketBokkingService(context);

            //Act
            var availbaleServices = ticketBookingService.GetAcailableTickects(date);

            //Assert
            Assert.Equal(2, availbaleServices.Count());
            Assert.Contains(availbaleServices, q => q.Id == 2);
            Assert.Contains(availbaleServices, q => q.Id == 3);
            Assert.DoesNotContain(availbaleServices, q => q.Id == 1);

        }

        [Fact]
        public void ShouldSaveTicketBooking()
        {
            //Arrange 
            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
                .UseInMemoryDatabase("ShouldSaveTest", b => b.EnableNullChecks(false))
                .Options;

            var ticketBooking = new TicketBooking { TickectID = 1, Date = new DateTime(2023, 08, 01) };

            //Act 
            using var context = new TicketingSolutionDbContext(dbOptions);
            var ticketBookingService = new TicketBokkingService(context);
            ticketBookingService.Save(ticketBooking);

            // Assert
            var bookings = context.TicketBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(ticketBooking.Date, booking.Date);

        }
           
    }
}