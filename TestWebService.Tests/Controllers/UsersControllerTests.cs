namespace TestWebService.Tests.Controllers
{
    using AutoMapper;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using TestWebService.Controllers;
    using TestWebService.DA;
    using TestWebService.DA.Models;
    using TestWebService.Models;

    public class UsersControllerTests
    {
        [SetUp]
        public void Setup() { }

        [TearDown]
        public void TearDown() { }

        [Test]
        public async Task Should_ReturnAll()
        {
            var users = new[] { new User { Id = 1, FirstName = "F", LastName = "L", Email = "E" } };
            var userDtos = new[] { new UserDto { Id = 1, FirstName = "F", LastName = "L", Email = "E" } };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.GetAll())
                .ReturnsAsync(users);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<User>, IEnumerable<UserDto>>(users))
                .Returns(userDtos);

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.GetUsers();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<UserDto>>>());

            repoMock.Verify(m => m.GetAll(), Times.Once());
            mapperMock.Verify(m => m.Map<IEnumerable<User>, IEnumerable<UserDto>>(users), Times.Once());
        }

        [Test]
        public async Task Should_ReturnById()
        {
            var userId = 5;
            var user = new User { Id = userId, FirstName = "F", LastName = "L", Email = "E" };
            var userDto = new UserDto { Id = userId, FirstName = "F", LastName = "L", Email = "E" };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.GetById(userId))
                .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
                .Returns(userDto);

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.GetUser(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<UserDto>>());

            repoMock.Verify(m => m.GetById(userId), Times.Once());
            mapperMock.Verify(m => m.Map<User, UserDto>(user), Times.Once());
        }

        [Test]
        public async Task Should_Not_ReturnById()
        {
            var userId = 5;

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.GetById(userId))
                .ReturnsAsync((User)null);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
                .Returns((UserDto)null);

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.GetUser(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundResult>());

            repoMock.Verify(m => m.GetById(userId), Times.Once());
            mapperMock.Verify(m => m.Map<User, UserDto>(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public async Task Should_Create()
        {
            var userId = 5;
            var userDto = new UserDto { FirstName = "F", LastName = "L", Email = "Email@email" };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
                .Returns((UserDto u) => { return MapToDbUser(u); });

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.Save(It.IsAny<User>()))
                .Returns((User u) =>
                {
                    u.Id = userId;
                    return Task.FromResult(u);
                });

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.PostUser(userDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<StatusCodeResult>());
            Assert.That((result as StatusCodeResult).StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            mapperMock.Verify(m => m.Map<UserDto, User>(It.IsAny<UserDto>()), Times.Once());
            repoMock.Verify(m => m.Save(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public async Task Should_Not_CreateWithModelStateErrors()
        {
            var mapperMock = new Mock<IMapper>();

            var repoMock = new Mock<IUserRepository>();

            var controller = new UsersController(repoMock.Object, mapperMock.Object);
            controller.ModelState.AddModelError("user.FirstName", "The FirstName field is required.");

            var result = await controller.PostUser(new UserDto());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<InvalidModelStateResult>());

            mapperMock.Verify(m => m.Map<UserDto, User>(It.IsAny<UserDto>()), Times.Never());
            repoMock.Verify(m => m.Save(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public async Task Should_Update()
        {
            var userId = 5;
            var userDto = new UserDto { Id = userId, FirstName = "F", LastName = "L", Email = "Email@email" };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
                .Returns((UserDto u) => { return MapToDbUser(u); });

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.Save(It.IsAny<User>()))
                .Returns((User u) =>
                {
                    return Task.FromResult(u);
                });

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.PutUser(userId, userDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<StatusCodeResult>());
            Assert.That((result as StatusCodeResult).StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            mapperMock.Verify(m => m.Map<UserDto, User>(It.IsAny<UserDto>()), Times.Once());
            repoMock.Verify(m => m.Save(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public async Task Should_Not_UpdateWithDiffIds()
        {
            var userId = 5;
            var otherUserId = 6;
            var userDto = new UserDto { Id = userId, FirstName = "F", LastName = "L", Email = "Email@email" };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
                .Returns((UserDto u) => { return MapToDbUser(u); });

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(m => m.Save(It.IsAny<User>()))
                .Returns((User u) =>
                {
                    return Task.FromResult(u);
                });

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.PutUser(otherUserId, userDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestResult>());

            mapperMock.Verify(m => m.Map<UserDto, User>(It.IsAny<UserDto>()), Times.Never());
            repoMock.Verify(m => m.Save(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public async Task Should_Not_UpdateWithModelStateErrors()
        {
            var userId = 5;
            var userDto = new UserDto { Id = userId };

            var mapperMock = new Mock<IMapper>();

            var repoMock = new Mock<IUserRepository>();

            var controller = new UsersController(repoMock.Object, mapperMock.Object);
            controller.ModelState.AddModelError("user.FirstName", "The FirstName field is required.");

            var result = await controller.PutUser(userId, userDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<InvalidModelStateResult>());

            mapperMock.Verify(m => m.Map<UserDto, User>(It.IsAny<UserDto>()), Times.Never());
            repoMock.Verify(m => m.Save(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public async Task Should_Delete()
        {
            var userId = 5;
            var repoMock = new Mock<IUserRepository>();

            var mapperMock = new Mock<IMapper>();

            var controller = new UsersController(repoMock.Object, mapperMock.Object);

            var result = await controller.DeleteUser(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkResult>());

            mapperMock.Verify(m => m.Map<It.IsAnyType, It.IsAnyType>(It.IsAny<It.IsAnyType>()), Times.Never());
            repoMock.Verify(m => m.DeleteById(userId), Times.Once());
        }

        private static User MapToDbUser(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
            };
        }
    }
}
