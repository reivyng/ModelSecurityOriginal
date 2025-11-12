using AutoMapper;
using Business.Implements;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace test.Business
{
    public class UserBusinessTests
    {
        private readonly Mock<IUserData> _mockUserData;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UserBusiness>> _mockLogger;
        private readonly UserBusiness _service;

        public UserBusinessTests()
        {
            _mockUserData = new Mock<IUserData>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UserBusiness>>();

            _service = new UserBusiness(
                _mockUserData.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCallDataCreateAndReturnDto()
        {
            // Arrange
            var dto = new UserDto
            {
                Id = 0,
                email = "test@example.com",
                password = "secret",
                person_id = 1,
                role_id = 1
            };

            var user = new User
            {
                Id = 1,
                email = dto.email,
                password = dto.password,
                person_id = dto.person_id,
                rol_id = dto.role_id
            };

            _mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);
            _mockUserData.Setup(d => d.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Id = user.Id, email = user.email });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.email.Should().Be(dto.email);

            _mockUserData.Verify(d => d.CreateAsync(It.IsAny<User>()), Times.Once);
            _mockMapper.Verify(m => m.Map<User>(dto), Times.Once);
        }
    }
}
