using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce_App.Areas.Identity.Data;
using Ecommerce_App.Controllers;
using Domain.DTO_s;
using Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class ReservationsControllerTests
{
    private readonly Mock<Infrastructure.Data.Ecommerce_AppContext> _dbContextMock = new Mock<Infrastructure.Data.Ecommerce_AppContext>();
    private readonly Mock<UserManager<Ecommerce_AppUser>> userManagerMock = new Mock<UserManager<Ecommerce_AppUser>>();
    private readonly Mock<IReservationService> reservationServiceMock = new Mock<IReservationService>();
    private readonly Mock<IRoomService> roomServiceMock = new Mock<IRoomService>();
    private readonly Mock<IRoomTypeService> roomTypeServiceMock = new Mock<IRoomTypeService>();
    private readonly Mock<IEscortService> escortServiceMock = new Mock<IEscortService>();
    private readonly Mock<ILoggerService> loggerMock = new Mock<ILoggerService>();
    [Fact]
    public async Task Index_ReturnsViewWithReservations()
    {
        // Arrange
        var reservationServiceMock = new Mock<IReservationService>();
        reservationServiceMock.Setup(service => service.GetAllReservations())
            .ReturnsAsync(new List<Reservation> { new Reservation() });

        var controller = new ReservationsController(
            _dbContextMock.Object,
            userManagerMock.Object,
            reservationServiceMock.Object,
            roomServiceMock.Object,
            roomTypeServiceMock.Object,
            escortServiceMock.Object,
            loggerMock.Object);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Reservation>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task CreateGET_ReturnsViewWithModel()
    {
        // Arrange
        var roomServiceMock = new Mock<IRoomService>();
        roomServiceMock.Setup(service => service.GetAllRoom())
            .ReturnsAsync(new List<Room> { new Room { RoomId = 1, RoomType = new LookUpProperty { TypeId = 1} } });

        var controller = new ReservationsController(
            _dbContextMock.Object,
            userManagerMock.Object,
            reservationServiceMock.Object,
            roomServiceMock.Object,
            roomTypeServiceMock.Object,
            escortServiceMock.Object,
            loggerMock.Object);

        // Act
        var result = await controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Reservation>(viewResult.Model);
        Assert.NotNull(model.Rooms);
        Assert.Single(model.Rooms);
        Assert.Equal("TestRoom", model.Rooms[0].Text);
    }

}
