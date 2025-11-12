using System.Threading.Tasks;
using Business.Implements;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto.Auth;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ModelSecurityOriginal.Tests
{
    public class AuthBusinessTests
    {
        [Fact]
        public async Task RegisterAsync_UserAlreadyExists_ReturnsFalse()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                first_name = "John",
                first_last_name = "Doe",
                phone_number = 1234567890,
                number_identification = 9876543210,
                email = "existing@domain.com",
                password = "pwd"
            };

            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.FindByEmailAsync(dto.email)).ReturnsAsync(new User { email = dto.email });

            var rolDataMock = new Mock<IRolData>();

            var auth = new AuthService(userDataMock.Object, rolDataMock.Object);

            // Act
            var result = await auth.RegisterAsync(dto);

            // Assert
            Assert.False(result);
            userDataMock.Verify(u => u.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_RoleExists_CreatesUserAndReturnsTrue()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                first_name = "Jane",
                first_last_name = "Roe",
                phone_number = 1112223333,
                number_identification = 4445556666,
                email = "new@domain.com",
                password = "secretpwd"
            };

            var existingRole = new Rol { Id = 1, type_rol = "User" };

            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.FindByEmailAsync(dto.email)).ReturnsAsync((User)null);
            userDataMock.Setup(u => u.CreateAsync(It.IsAny<User>())).ReturnsAsync((User u) =>
            {
                // simulate DB assigning Id
                u.Id = 10;
                return u;
            }).Verifiable();

            var rolDataMock = new Mock<IRolData>();
            rolDataMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingRole);

            var auth = new AuthService(userDataMock.Object, rolDataMock.Object);

            // Act
            var result = await auth.RegisterAsync(dto);

            // Assert
            Assert.True(result);
            userDataMock.Verify(u => u.CreateAsync(It.Is<User>(x => x.email == dto.email && x.rol_id == existingRole.Id)), Times.Once);
            rolDataMock.Verify(r => r.CreateAsync(It.IsAny<Rol>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_RoleMissing_CreatesRoleAndUser_ReturnsTrue()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                first_name = "Alice",
                first_last_name = "Smith",
                phone_number = 2223334444,
                number_identification = 5556667777,
                email = "alice@domain.com",
                password = "mypassword"
            };

            var createdRole = new Rol { Id = 3, type_rol = "User" };

            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.FindByEmailAsync(dto.email)).ReturnsAsync((User)null);
            User captured = null;
            userDataMock.Setup(u => u.CreateAsync(It.IsAny<User>())).ReturnsAsync((User u) =>
            {
                captured = u;
                u.Id = 20;
                return u;
            }).Verifiable();

            var rolDataMock = new Mock<IRolData>();
            rolDataMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Rol)null);
            rolDataMock.Setup(r => r.CreateAsync(It.IsAny<Rol>())).ReturnsAsync(createdRole).Verifiable();

            var auth = new AuthService(userDataMock.Object, rolDataMock.Object);

            // Act
            var result = await auth.RegisterAsync(dto);

            // Assert
            Assert.True(result);
            rolDataMock.Verify(r => r.CreateAsync(It.Is<Rol>(rr => rr.type_rol == "User")), Times.Once);
            userDataMock.Verify(u => u.CreateAsync(It.IsAny<User>()), Times.Once);
            Assert.NotNull(captured);
            Assert.Equal(createdRole.Id, captured.rol_id);
            // password must be hashed (not equal to plain)
            Assert.NotEqual(dto.password, captured.password);
            Assert.True(BCrypt.Net.BCrypt.Verify(dto.password, captured.password));
        }
    }
}
