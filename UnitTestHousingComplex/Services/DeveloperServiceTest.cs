using System.Linq.Expressions;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
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

    [Fact]
    public async Task Should_ReturnDeveloper_When_GetForMeeting()
    {
        var developer = new Developer
        {
            Id = Guid.NewGuid(),
            Name = "Emerald",
            PhoneNumber = "098213",
            UserCredentialId = Guid.NewGuid(),
            Address = "Bogor",
            UserCredential = new UserCredential
            {
                Id = Guid.NewGuid(),
                Email = "dev@gmail.com",
                Password = "password",
                RoleId = Guid.NewGuid(),
                Role = new Role()
            },
            Housing = new Housing()
            {
                Id = Guid.NewGuid(),
                Name = "Emerald",
                DeveloperId = Guid.NewGuid(),
                Address = "Cilebut",
                OpenTime = "Weekday",
                City = "Bogor",
                Meets = new List<Meet>()
                {
                    new Meet()
                    {
                        Id = Guid.NewGuid(),
                        MeetDate = "2023-02-04",
                        IsMeet = false,
                        HousingId = Guid.NewGuid(),
                        CustomerId = Guid.NewGuid(),
                        Housing = new Housing(),
                        Customer = new Customer()
                    }
                },
                Developer = new Developer()
            }
        };
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Developer, bool>>>(),
            It.IsAny<string[]>())).ReturnsAsync(developer);

        var result = await _developerService.GetForMeeting("dev@gmail.com");
        Assert.Equal(developer.Id,result.Id);
        Assert.Equal(developer.Name,result.Name);
        Assert.Equal(developer.PhoneNumber,result.PhoneNumber);
        Assert.Equal(developer.UserCredentialId,result.UserCredentialId);
        Assert.Equal(developer.Address,result.Address);
        Assert.Equal(developer.UserCredential.Id,result.UserCredential.Id);
        Assert.Equal(developer.UserCredential.RoleId,result.UserCredential.RoleId);
        Assert.NotNull(developer.UserCredential);
    }

    [Fact]
    public async Task Should_ThrowNotFoundException_When_GetForMeetingIsNull()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _developerService.GetForMeeting(String.Empty));
    }

}