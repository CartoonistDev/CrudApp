using CrudApp.IServices;
using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrudTest;

public class UsersServiceTest
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _controller;

    public UsersServiceTest()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public void CreateUser_ValidUser_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Name = "John Doe", Age = 30 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CreateUser_InvalidUser_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Name = "", Age = 30 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new ArgumentException("Name cannot be null or empty"));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void CreateUser_DatabaseError_ReturnsInternalServerError()
    {
        // Arrange
        var user = new User { Name = "John Doe", Age = 30 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new Exception("Database error"));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void GetAllUsers_ReturnsOkResultWithUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Age = 30 },
            new User { Id = 2, Name = "Jane Doe", Age = 25 }
        };
        _mockUserService.Setup(s => s.GetAllUsers()).Returns(users);

        // Act
        var result = _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
    }

    [Fact]
    public void GetAllUsers_DatabaseError_ReturnsInternalServerError()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetAllUsers()).Throws(new Exception("Database error"));

        // Act
        var result = _controller.GetAllUsers();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void UpdateUser_ValidUser_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John Doe", Age = 31 };
        _mockUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));

        // Act
        var result = _controller.UpdateUser(1, user);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void UpdateUser_InvalidUser_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Id = 1, Name = "", Age = 31 };
        _mockUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new ArgumentException("Name cannot be null or empty"));

        // Act
        var result = _controller.UpdateUser(1, user);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UpdateUser_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var user = new User { Id = 999, Name = "John Doe", Age = 31 };
        _mockUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new KeyNotFoundException("User with ID 999 not found"));

        // Act
        var result = _controller.UpdateUser(999, user);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void DeleteUser_ValidId_ReturnsOkResult()
    {
        // Arrange
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<int>()));

        // Act
        var result = _controller.DeleteUser(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void DeleteUser_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<int>()))
            .Throws(new ArgumentException("Invalid user ID"));

        // Act
        var result = _controller.DeleteUser(0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DeleteUser_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<int>()))
            .Throws(new KeyNotFoundException("User with ID 999 not found"));

        // Act
        var result = _controller.DeleteUser(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void CreateUser_NullUser_ReturnsBadRequest()
    {
        // Act
        var result = _controller.CreateUser(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void GetAllUsers_VerifyReturnedUsers()
    {
        // Arrange
        var users = new List<User>
    {
        new User { Id = 1, Name = "John Doe", Age = 30 },
        new User { Id = 2, Name = "Jane Doe", Age = 25 }
    };
        _mockUserService.Setup(s => s.GetAllUsers()).Returns(users);

        // Act
        var result = _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
        Assert.Equal(users, returnedUsers);
    }

    [Fact]
    public void UpdateUser_VerifyCorrectIdPassed()
    {
        // Arrange
        var userId = 5;
        var user = new User { Id = userId, Name = "John Doe", Age = 31 };
        _mockUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));

        // Act
        _controller.UpdateUser(userId, user);

        // Assert
        _mockUserService.Verify(s => s.UpdateUser(userId, user.Name, user.Age), Times.Once);
    }

    [Fact]
    public void UpdateUser_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var userId = 5;
        var user = new User { Id = 6, Name = "John Doe", Age = 31 };

        // Act
        var result = _controller.UpdateUser(userId, user);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DeleteUser_VerifyCorrectIdPassed()
    {
        // Arrange
        var userId = 5;
        _mockUserService.Setup(s => s.DeleteUser(It.IsAny<int>()));

        // Act
        _controller.DeleteUser(userId);

        // Assert
        _mockUserService.Verify(s => s.DeleteUser(userId), Times.Once);
    }

    [Fact]
    public void CreateUser_MaxAgeBoundary_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Name = "Old John", Age = 150 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CreateUser_AgeAboveMax_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Name = "Too Old John", Age = 151 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new ArgumentOutOfRangeException("Age must be between 0 and 150"));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void CreateUser_NegativeAge_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Name = "Negative John", Age = -1 };
        _mockUserService.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<int>()))
            .Throws(new ArgumentOutOfRangeException("Age must be between 0 and 150"));

        // Act
        var result = _controller.CreateUser(user);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
