using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class DeveloperServiceTest
{
    private readonly Mock<IRepository<Developer>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly IDeveloperService _developerService;

    public DeveloperServiceTest()
    {
        _mockRepository = new Mock<IRepository<Developer>>();
        _mockPersistence = new Mock<IPersistence>();
        _developerService = new DeveloperService(_mockRepository.Object, _mockPersistence.Object);
    }

    [Fact]
    public async Task Should_ReturnDeveloper_When_CreateDeveloper()
    {
        var developer = new Developer
        {
            Id = Guid.NewGuid(),
            Name = "Proland",
            PhoneNumber = "0123902139",
            UserCredentialId = Guid.NewGuid(),
            Address = "Pajeleran",
            UserCredential = new UserCredential()
        };

        _mockRepository.Setup(repo => repo.Save(It.IsAny<Developer>()))
            .ReturnsAsync(developer);
        _mockPersistence.Setup(per => per.SaveChangesAsync());

        var result = await _developerService.CreateDeveloper(developer);
        
        Assert.Equal(developer,result);
    }
    
}