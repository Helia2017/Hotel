using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO_s;
using Domain.Interface;
using Ecommerce_App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;
using orm = Infrastructure.Data;

public class RoomsControllerTests
{
    private readonly Mock<orm.Ecommerce_AppContext> _dbContextMock = new Mock<orm.Ecommerce_AppContext>();
    private readonly Mock<IRoomService> _roomServiceMock = new Mock<IRoomService>();
    private readonly Mock<ILookUpTypeService> _lookUpTypeServiceMock = new Mock<ILookUpTypeService>();
    private readonly Mock<ILookUpPropertyService> _lookUpPropertyServiceMock = new Mock<ILookUpPropertyService>();
    private readonly Mock<IRoomImageService> _roomImageServiceMock = new Mock<IRoomImageService>();
    private readonly Mock<ILoggerService> _loggerMock = new Mock<ILoggerService>();

    [Fact]
    public async Task Index_ReturnsViewWithListOfRooms()
    {
        // Arrange
        var rooms = new List<Room> { new Room() }; // Provide sample data
        _roomServiceMock.Setup(s => s.GetAllRoom()).ReturnsAsync(rooms);

        var controller = new RoomsController(
            _dbContextMock.Object,
            _roomServiceMock.Object,
            _lookUpTypeServiceMock.Object,
            _roomImageServiceMock.Object,
                        _lookUpPropertyServiceMock.Object,
            _loggerMock.Object
        );

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Room>>(viewResult.Model);
        Assert.Single(model); // Check if the returned model has one item
        _roomServiceMock.Verify(s => s.GetAllRoom(), Times.Once);
    }

    [Fact]
    public async Task Create_GET_ReturnsViewWithRoomTypes()
    {
        // Arrange
        var roomTypes = new List<LookUpProperty> { new LookUpProperty() }; // Provide sample data
        _lookUpPropertyServiceMock.Setup(s => s.GetAllLookUpProperty()).ReturnsAsync(roomTypes);

        var controller = new RoomsController(
            _dbContextMock.Object,
            _roomServiceMock.Object,
            _lookUpTypeServiceMock.Object,
            _roomImageServiceMock.Object,
                        _lookUpPropertyServiceMock.Object,
            _loggerMock.Object
        );

        // Act
        var result = await controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var typeOptions = Assert.IsAssignableFrom<IEnumerable<SelectListItem>>(viewResult);
        Assert.Single(typeOptions); // Check if the ViewBag has one item
        _lookUpPropertyServiceMock.Verify(s => s.GetAllLookUpProperty(), Times.Once);
    }


    [Fact]
    public async Task Delete_DeletesRoomAndRedirectsToIndex()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock.Setup(s => s.Delete(roomId));

        var controller = new RoomsController(
            _dbContextMock.Object,
            _roomServiceMock.Object,
            _lookUpTypeServiceMock.Object,
            _roomImageServiceMock.Object,
                        _lookUpPropertyServiceMock.Object,
            _loggerMock.Object
        );

        // Act
        var result = await controller.Delete(roomId);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _roomServiceMock.Verify(s => s.Delete(roomId), Times.Once);
    }
}
