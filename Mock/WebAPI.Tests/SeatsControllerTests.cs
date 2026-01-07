using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{

    Mock<SeatsController> seatsControllerMock;
    Mock<SeatsService> SeatsServiceMock;

    public SeatsControllerTests()
    {
        SeatsServiceMock = new Mock<SeatsService>();
        seatsControllerMock = new Mock<SeatsController>(SeatsServiceMock.Object) { CallBase = true };

        seatsControllerMock.Setup(c => c.UserId).Returns("11111");
    }

    [TestMethod]
    public void ReserveSeat()
    {
        Seat seat = new Seat() { Id = 1, Number = 1 };

        SeatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Returns(seat);

        var action = seatsControllerMock.Object.ReserveSeat(seat.Number);

        var result = action.Result as OkObjectResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeat_SeatAlreadyTaken()
    {
        SeatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        var action = seatsControllerMock.Object.ReserveSeat(1);

        var result = action.Result as UnauthorizedResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeat_SeatOutOfBounds()
    {
        SeatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var seatNumber = 1;

        var action = seatsControllerMock.Object.ReserveSeat(seatNumber);

        var result = action.Result as NotFoundObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find " + seatNumber, result.Value);

    }

    [TestMethod]
    public void ReserveSeat_UserAlreadySeated()
    {
        SeatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var action = seatsControllerMock.Object.ReserveSeat(1);

        var result = action.Result as BadRequestResult;
        Assert.IsNotNull(result);
    }
}