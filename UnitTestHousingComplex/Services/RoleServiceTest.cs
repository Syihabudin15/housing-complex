using System.Linq.Expressions;
using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;

namespace UnitTestHousingComplex.Services;

public class RoleServiceTest
{
    private readonly Mock<IRepository<Role>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly IRoleService _roleService;

    public RoleServiceTest()
    {
        _mockRepository = new Mock<IRepository<Role>>();
        _mockPersistence = new Mock<IPersistence>();
        _roleService = new RoleService(_mockRepository.Object, _mockPersistence.Object);
    }
    
    [Fact]
    public async Task Should_ReturnRole_When_GetOrSave()
    {
        Role roleFindMock = null;
        var roleSaveMock = new Role
        {
            Id = Guid.NewGuid(),
            ERole = ERole.Customer
        };

        _mockRepository.Setup(repository => repository.Find(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(roleFindMock);
        _mockRepository.Setup(repository => repository.Save(It.IsAny<Role>())).ReturnsAsync(roleSaveMock);
        _mockPersistence.Setup(persistence => persistence.SaveChangesAsync());

        var result = await _roleService.GetOrSave(ERole.Customer);
        
        Assert.Equal(roleSaveMock,result);
    }
    
    [Fact]
    public async Task Should_ReturnRole_When_GetOrSaveERoleFound()
    {
        // await Assert.ThrowsAsync<NotFoundException>(async () =>
        //     await _roleService.GetOrSave());
        Role roleFindMock = new Role
        {
            Id = Guid.NewGuid(),
            ERole = ERole.Customer
        };

        _mockRepository.Setup(repository => repository.Find(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(roleFindMock);

        var result = await _roleService.GetOrSave(ERole.Customer);
        Assert.Equal(roleFindMock,result);
        Assert.NotNull(roleFindMock.Id.ToString());
    }
}