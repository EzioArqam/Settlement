using SettlementService.Entities;

namespace SettlementService.UnitTests;

public class BookingEntityTests
{
    //Sample Tests for checking purposes
    [Fact]
    public void HasName_NameNotEmpty_True()
    {
        //arrange
        var booking = new Booking{Id=Guid.NewGuid(), Name="John"};
        //act
        var result = booking.HasName();
        //assert
        Assert.True(result);

    }
    [Fact]
    public void HasName_NameEmpty_False()
    {
        //arrange
        var booking = new Booking{Id=Guid.NewGuid(), Name=""};
        //act
        var result = booking.HasName();
        //assert
        Assert.False(result);

    }
}