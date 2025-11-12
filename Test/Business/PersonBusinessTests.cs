using AutoMapper;
using Business.Implements;
using Data.Interfaces;
using Data.Interface;
using Entity.Dto;
using Entity.Model;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace test.Business
{
    public class PersonBusinessTests
    {
        private readonly Mock<IPersonData> _mockPersonData;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PersonBusiness>> _mockLogger;
        private readonly PersonBusiness _service;

        public PersonBusinessTests()
        {
            _mockPersonData = new Mock<IPersonData>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PersonBusiness>>();

            _service = new PersonBusiness(
                _mockPersonData.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCallDataCreateAndReturnDto()
        {
            // Arrange
            var dto = new PersonDto
            {
                Id = 0,
                first_name = "Juan",
                first_last_name = "Perez",
                phone_number = 300123456,
                number_identification = 12345678
            };

            var person = new Person
            {
                Id = 1,
                first_name = dto.first_name,
                first_last_name = dto.first_last_name,
                phone_number = dto.phone_number,
                number_identification = dto.number_identification
            };

            _mockMapper.Setup(m => m.Map<Person>(dto)).Returns(person);
            _mockPersonData.Setup(d => d.CreateAsync(It.IsAny<Person>())).ReturnsAsync(person);
            _mockMapper.Setup(m => m.Map<PersonDto>(person)).Returns(new PersonDto { Id = person.Id, first_name = person.first_name });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.first_name.Should().Be(dto.first_name);

            _mockPersonData.Verify(d => d.CreateAsync(It.IsAny<Person>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Person>(dto), Times.Once);
        }
    }
}
