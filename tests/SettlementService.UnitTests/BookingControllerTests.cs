using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SettlementService.DTOs;
using SettlementService.Entities;
using SettlementService.Repositories;

namespace SettlementService.UnitTests;

public class BookingControllerTests
{
    private readonly Mock<IBookingRepository> _bookingrepo;
    private readonly Fixture _fixture;
    private readonly BookingController _controller;
    private readonly IMapper _mapper;
    public BookingControllerTests(){
        _fixture = new Fixture();
        _bookingrepo = new Mock<IBookingRepository>();

        // We are using MappingProfiles generated for BookingService, not actually creating a mock of IMapper

        var mockMapper = new MapperConfiguration(mc =>{
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;
        _mapper = new Mapper(mockMapper);
        _controller = new BookingController(_mapper,_bookingrepo.Object);
    }

    // _bookingrepo.Setup(repo => repo.GetBookingsAtTime(It.IsAny<TimeSpan>()));
    //     _bookingrepo.Setup(repo => repo.AddBooking(It.IsAny<Booking>()));
    //     _bookingrepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

    // Test for invalid time format:
    [Fact]
    public async Task CreateBooking_InvalidTimeFormat_ReturnsBadRequest()
    {
        // Arrange
        
        var bookingDTO = new CreateBookingDTO { BookingTime = "invalid", Name = "John Smith" };

        // Act
        var result = await _controller.CreateBooking(bookingDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    // Test for booking time out of business hours:
    [Fact]
    public async Task CreateBooking_OutOfBusinessHours_ReturnsBadRequest()
    {
        // Arrange
        var bookingDTO = new CreateBookingDTO { BookingTime = "08:00", Name = "John Smith" };

        // Act
        var result = await _controller.CreateBooking(bookingDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBooking_AllSettlementsReserved_ReturnsConflict()
    {
        // Arrange

        //fixture can be used here to create different times as well
        var bookingDTO = new CreateBookingDTO { BookingTime = "10:00", Name = "John Smith" };
        _bookingrepo.Setup(r => r.GetBookingsAtTime(It.IsAny<TimeSpan>()))
        .ReturnsAsync(new List<Booking> { new Booking(), new Booking(), new Booking(), new Booking() });
        _bookingrepo.Setup(repo => repo.AddBooking(It.IsAny<Booking>()));
        _bookingrepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateBooking(bookingDTO);

        // Assert
        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    // Test for successful booking creation:
    [Fact]
    public async Task CreateBooking_ValidRequest_ReturnsOkWithBookingId()
    {
        // Arrange
        var bookingDTO = new CreateBookingDTO { BookingTime = "10:00", Name = "John Smith" };
        _bookingrepo.Setup(r => r.GetBookingsAtTime(It.IsAny<TimeSpan>())).ReturnsAsync(new List<Booking>());
        _bookingrepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        _bookingrepo.Setup(repo => repo.AddBooking(It.IsAny<Booking>()));
        _bookingrepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateBooking(bookingDTO);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }


}
