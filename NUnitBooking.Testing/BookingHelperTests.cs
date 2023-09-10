using Moq;
using System.Collections.Generic;
using System.Linq;
namespace NUnitBooking.Testing;

public class BookingHelperTests: IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUNitOfWork;
    private readonly BookingHelper _bookingHelper;

    public BookingHelperTests()
    {
        _mockUNitOfWork = new Mock<IUnitOfWork>();
        _bookingHelper = new BookingHelper(_mockUNitOfWork.Object);
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void  OverlappingBookingsExist_BookingIsCancelled_ReturnsEmptyString()
    {
        var booking = new Booking{Status = "Cancelled"};
        var result = _bookingHelper.OverlappingBookingsExist(booking);

        Assert.That( result,Is.EqualTo(string.Empty));
    }

    [Test]
    public void OverlappingBookingsExist_BookingOverlaps_ReturnsBookingReference()
    {
        var booking = new Booking 
        { 
            Id = 2,
            ArrivalDate = new DateTime(2023, 1, 10),
            DepartureDate = new DateTime(2023, 1, 15),
            Status = "Active"
        };

        _mockUNitOfWork.Setup(u => u.Query<Booking>()).Returns(new List<Booking>
        {
            new Booking
            {
                Id = 1,
                ArrivalDate = new DateTime(2023, 1, 12),
                DepartureDate = new DateTime(2023, 1, 16),
                Status = "Active",
                Reference = "Ref1234"
            }
        }.AsQueryable());

        var result = _bookingHelper.OverlappingBookingsExist(booking);

        Assert.That( result,Is.EqualTo("Ref1234"));
    }
    public void Dispose()
    {
        // Cleanup logic here if needed.
        // It will be called after each test method is executed.
    }
}