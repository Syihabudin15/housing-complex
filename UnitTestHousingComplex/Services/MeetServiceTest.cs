using System.Linq.Expressions;
using HousingComplex.Entities;
using HousingComplex.Exceptions;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class MeetServiceTest
{
    private readonly Mock<IRepository<Meet>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<IHousingService> _mockHousingService;
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly Mock<IDeveloperService> _mockDeveloperService;
    private readonly Mock<IMeetService> _mockMeetService;
    private readonly IMeetService _meetService;

    public MeetServiceTest()
    {
        _mockRepository = new Mock<IRepository<Meet>>();
        _mockPersistence = new Mock<IPersistence>();
        _mockHousingService = new Mock<IHousingService>();
        _mockCustomerService = new Mock<ICustomerService>();
        _mockDeveloperService = new Mock<IDeveloperService>();
        _mockMeetService = new Mock<IMeetService>();
        _meetService = new MeetService(_mockRepository.Object, _mockPersistence.Object, _mockHousingService.Object,
            _mockCustomerService.Object, _mockDeveloperService.Object);
    }

    [Fact]
    public async Task Should_ReturnMeetResponse_When_CreateMeetScheduleWeekday()
    {
        _housing.OpenTime = "weekend";
        _meet.MeetDate = "2023-02-04";
        _mockHousingService.Setup(house => house.GetById(It.IsAny<string>()))
            .ReturnsAsync(_housing);
        _mockCustomerService.Setup(cust => cust.GetById(It.IsAny<string>()))
            .ReturnsAsync(_customer);
        _mockRepository.Setup(repo => repo.Save(It.IsAny<Meet>()))
            .ReturnsAsync(_meet);
        _mockPersistence.Setup(per => per.SaveChangesAsync());
        
        var result = await _meetService.CreateMeetSchedule(_meet);
        
        Assert.Equal(_meet.MeetDate,result.MeetDate);
        Assert.Equal(_meet.Id.ToString(),result.Id);
        Assert.Equal(_meet.IsMeet,result.IsMeet);
        Assert.Equal(_meet.Housing.Name,result.Housing.Name);
        Assert.Equal(_meet.Housing.Id.ToString(),result.Housing.Id);
        Assert.Equal(_meet.Housing.Address,result.Housing.Address);
        Assert.Equal(_meet.Housing.OpenTime,result.Housing.OpenTime);
        Assert.Equal(_meet.Housing.City,result.Housing.City);
        Assert.NotNull(_meet.Housing.DeveloperId);
        Assert.NotNull(_meet.Housing.Meets);
        Assert.NotNull(_meet.Housing.Developer);
        Assert.Equal(_meet.Customer.FirstName,result.Customer.FisrtName);
        Assert.Equal(_meet.Customer.Id.ToString(),result.Customer.Id);
        Assert.Equal(_meet.Customer.LastName,result.Customer.LastName);
        Assert.Equal(_meet.Customer.City,result.Customer.City);
        Assert.Equal(_meet.Customer.PostalCode,result.Customer.PostalCode);
        Assert.Equal(_meet.Customer.Address,result.Customer.Address);
        Assert.Equal(_meet.Customer.PhoneNumber,result.Customer.PhoneNumber);
        Assert.NotNull(_meet.Customer.UserCredentialId);
        Assert.NotNull(_meet.Customer.UserCredential);
        Assert.NotNull(_meet.Customer.Meet);
    }
    [Fact]
    public async Task Should_ReturnMeetResponse_When_CreateMeetScheduleWeekend()
    {

        _housing.OpenTime = "weekday";
        _meet.MeetDate = "2023-02-06";
        _mockHousingService.Setup(house => house.GetById(It.IsAny<string>()))
            .ReturnsAsync(_housing);
        _mockCustomerService.Setup(cust => cust.GetById(It.IsAny<string>()))
            .ReturnsAsync(_customer);
        _mockRepository.Setup(repo => repo.Save(It.IsAny<Meet>()))
            .ReturnsAsync(_meet);
        _mockPersistence.Setup(per => per.SaveChangesAsync());
        
        var result = await _meetService.CreateMeetSchedule(_meet);
        
        Assert.Equal(_meet.MeetDate,result.MeetDate);
        Assert.Equal(_meet.Id.ToString(),result.Id);
        Assert.Equal(_meet.IsMeet,result.IsMeet);
        Assert.Equal(_meet.Housing.Name,result.Housing.Name);
        Assert.Equal(_meet.Housing.Id.ToString(),result.Housing.Id);
        Assert.Equal(_meet.Housing.Address,result.Housing.Address);
        Assert.Equal(_meet.Housing.OpenTime,result.Housing.OpenTime);
        Assert.Equal(_meet.Housing.City,result.Housing.City);
        Assert.NotNull(_meet.Housing.DeveloperId);
        Assert.NotNull(_meet.Housing.Meets);
        Assert.NotNull(_meet.Housing.Developer);
        Assert.Equal(_meet.Customer.FirstName,result.Customer.FisrtName);
        Assert.Equal(_meet.Customer.Id.ToString(),result.Customer.Id);
        Assert.Equal(_meet.Customer.LastName,result.Customer.LastName);
        Assert.Equal(_meet.Customer.City,result.Customer.City);
        Assert.Equal(_meet.Customer.PostalCode,result.Customer.PostalCode);
        Assert.Equal(_meet.Customer.Address,result.Customer.Address);
        Assert.Equal(_meet.Customer.PhoneNumber,result.Customer.PhoneNumber);
        Assert.NotNull(_meet.Customer.UserCredentialId);
        Assert.NotNull(_meet.Customer.UserCredential);
        Assert.NotNull(_meet.Customer.Meet);
    }
    [Fact]
    public async Task Should_ThrowScheduleNotFoundException_When_CreateMeetScheduleWeekdayNotFound()
    {
        _housing.OpenTime = "weekend";
        _meet.MeetDate = "2023-02-06";
        _mockHousingService.Setup(house => house.GetById(It.IsAny<string>()))
            .ReturnsAsync(_housing);
        _mockCustomerService.Setup(cust => cust.GetById(It.IsAny<string>()))
            .ReturnsAsync(_customer);


        await Assert.ThrowsAsync<ScheduleNotFoundException>(() => 
            _meetService.CreateMeetSchedule(_meet));
    }
    [Fact]
    public async Task Should_ThrowScheduleNotFoundException_When_CreateMeetScheduleWeekendNotFound()
    {

        _housing.OpenTime = "weekday";
        _meet.MeetDate = "2023-02-04";
        _mockHousingService.Setup(house => house.GetById(It.IsAny<string>()))
            .ReturnsAsync(_housing);
        _mockCustomerService.Setup(cust => cust.GetById(It.IsAny<string>()))
            .ReturnsAsync(_customer);


        await Assert.ThrowsAsync<ScheduleNotFoundException>(() => 
            _meetService.CreateMeetSchedule(_meet));
    }

    [Fact]
    public async Task Should_ReturnMeetResponse_When_UpdateStatusMeet()
    {

        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<Meet, bool>>>(),It.IsAny<string[]>()))
            .ReturnsAsync(_meet);
        _mockPersistence.Setup(per => per.SaveChangesAsync());
        var result = await _meetService.UpdateStatusMeet(Guid.NewGuid().ToString());
        Assert.Equal(_meet.Id.ToString(),result.Id);
        
    }

    [Fact]
    public async Task Should_ThrowNotFoundException_When_UpdateStatusMeetingIsNull()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _meetService.UpdateStatusMeet(String.Empty));
    }

    [Fact]
    public async Task Should_ReturnPageResponseMeetResponse_When_GetAllSchedule()
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
                        Housing = _housing,
                        Customer = _customer
                    }
                },
                Developer = new Developer()
            }
        };
        _mockDeveloperService.Setup(dev => dev.GetForMeeting(It.IsAny<string>()))
            .ReturnsAsync(developer);

        var result = await _meetService.GetAllSchedule(1, 5, "dev@gmail.com");
        Assert.Equal(developer.Housing.Meets.Count,result.Content.Count);
    }

    private static Housing _housing = new Housing()
    {
        Id = Guid.NewGuid(),
        Name = "Emerald",
        DeveloperId = Guid.NewGuid(),
        Address = "Cilebut",
        OpenTime = "Weekday",
        City = "Bogor",
        Meets = new List<Meet>(),
        Developer = new Developer()
    };

    private static Customer _customer = new Customer
    {
        Id = Guid.NewGuid(),
        FirstName = "Achmad",
        LastName = "Fikri",
        City = "Bogor",
        PostalCode = "16913",
        Address = "Cibinong",
        PhoneNumber = "091283",
        UserCredentialId = Guid.NewGuid(),
        UserCredential = new UserCredential(),
        Meet = new List<Meet>()
    };

    private static Meet _meet = new Meet()
    {
        Id = Guid.NewGuid(),
        MeetDate = "2023-02-04",
        IsMeet = false,
        HousingId = Guid.NewGuid(),
        CustomerId = Guid.NewGuid(),
        Housing = _housing,
        Customer = _customer
    };
}