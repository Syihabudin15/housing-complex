using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class CustomerServiceTest
{
    private readonly Mock<IRepository<Customer>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly ICustomerService _customerService;

    public CustomerServiceTest()
    {
        _mockRepository = new Mock<IRepository<Customer>>();
        _mockPersistence = new Mock<IPersistence>();
        _customerService = new CustomerService(_mockRepository.Object, _mockPersistence.Object);
    }

    [Fact]
    public async Task Should_ReturnCustomer_When_CreateCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "achmad",
            LastName = "fikri",
            City = "bogor",
            PostalCode = "16913",
            Address = "pajeleran gunung",
            PhoneNumber = "08123213412",
            UserCredentialId = Guid.NewGuid(),
            UserCredential = new UserCredential(),
            Meet = new List<Meet>()
        };

        _mockRepository.Setup(repo => repo.Save(It.IsAny<Customer>()))
            .ReturnsAsync(customer);

        _mockPersistence.Setup(per => per.SaveChangesAsync());

        var result = await _customerService.CreateCustomer(customer);
        
        Assert.Equal(customer,result);
    }
}