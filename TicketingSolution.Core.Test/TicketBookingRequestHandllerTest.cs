using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Enums;
using TicketingSolution.Core.Handller;
using TicketingSolution.Core.Model;
using TicketingSolution.Domain.Domain;
using Xunit;

namespace TicketingSolution.Core.Test
{
    public class TicketBookingRequestHandllerTest
    {
        private readonly TicketBookingRequestHandller _handller;
        private readonly TicketBookingRequest _Request;
        private readonly Mock<ITicketBookingService> _tickectBookingServiceMock;
        private List<Ticket> _availableTickets;

        public TicketBookingRequestHandllerTest()
        {
            //Arrange
            _Request = new TicketBookingRequest()
            {
                Name = "Test Name",
                Family = "Test Family",
                Email = "TestEmail@gmail.com",
                Date = DateTime.Now

            };

            _availableTickets = new List<Ticket>() { new Ticket() { Id = 1 } };//In order not to return null
            //چون باید همون اول ولیو بگیرد همینجا ستاپش انجام میشه 
            _tickectBookingServiceMock = new Mock<ITicketBookingService>();
            _tickectBookingServiceMock.Setup(q => q.GetAcailableTickects(_Request.Date))
                .Returns(_availableTickets);

            _handller = new TicketBookingRequestHandller(_tickectBookingServiceMock.Object);

        }
        [Fact]
        public void ShouldReturnTicketBookingResponseWithRequestValues()
        {


            //Act 
            ServiceBookingResult result = _handller.BookService(_Request);

            //Assert

            //Assert.NotNull(result);
            //Assert.Equal(result.Name, bookingReqest.Name);
            //Assert.Equal(result.Family, bookingReqest.Family);
            //Assert.Equal(result.Email, bookingReqest.Email);


            //Assert by shouldly
            result.ShouldNotBeNull();
            result.Name.ShouldBe(_Request.Name);
            result.Family.ShouldBe(_Request.Family);
            result.Email.ShouldBe(_Request.Email);
        }

        [Fact]
        public void ShouldThrowExcetionForNullRequest()
        {

            var exception = Should.Throw<ArgumentException>(() => _handller.BookService(null));
            exception.ParamName.ShouldBe("bookingReqest");

            //Assert.Throws<ArgumentException>(() => _handller.BookService(null));
        }

        [Fact]
        public void ShouldSaveTicketBookingRequest()
        {
            TicketBooking saveBooking = null;
            _tickectBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
                .Callback<TicketBooking>(booking =>
            {
                saveBooking = booking;
            });
            _handller.BookService(_Request);


            _tickectBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);
            saveBooking.ShouldNotBeNull();
            saveBooking.Name.ShouldBe(_Request.Name);
            saveBooking.Family.ShouldBe(_Request.Family);
            saveBooking.Email.ShouldBe(_Request.Email);
            saveBooking.TickectID.ShouldBe(_availableTickets.First().Id);
        }

        [Fact]
        public void ShouldNotSaveTicketBookingRequestIfNoneAvailable()
        {
            _availableTickets.Clear();
            _handller.BookService(_Request);
            _tickectBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void ShouldReturnSuccessOrFailureFlagInResult(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTickets.Clear();
            }
            var result = _handller.BookService(_Request);
            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void ShouldReturnTicketBookingInResult(int? tickectBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTickets.Clear();
            }
            else
            {
                _tickectBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
                  .Callback<TicketBooking>(x =>
                  {
                      TicketBooking.Id = tickectBookingId.Value;
                  });
            }
            var result = _handller.BookService(_Request);
            result.TicketBookingId.ShouldBe(tickectBookingId);
        }
    }
}
