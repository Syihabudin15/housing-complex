using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestHousingComplex.Services
{
    public class HouseTypeServiceTest
    {
        private readonly Mock<IRepository<HouseType>> _repository;
        private readonly Mock<IPersistence> _persistence;
        private readonly IHouseTypeService _service;
        private readonly List<HouseType> houseTypes = new List<HouseType>
            {
                new()
                {
                Id = Guid.NewGuid(),
                Name = "Type 30/70",
                Description = "Description",
                Price = 80000000,
                StockUnit = 20,
                HousingId = Guid.NewGuid(),
                SpesificationId = Guid.NewGuid(),
                ImageHouseTypeId = Guid.NewGuid()
                }
            };

        public HouseTypeServiceTest()
        {
            _repository = new Mock<IRepository<HouseType>>();
            _persistence = new Mock<IPersistence>();
            _service = new HouseTypeService(_persistence.Object, _repository.Object);
        }

        [Fact]
        public async Task Should_ReturnHouseType_When_CreateHouseType()
        {
            var houseType = new HouseType
            {
                Id = Guid.NewGuid(),
                Name = "Type 30/70",
                Description = "Description",
                Price = 80000000,
                StockUnit = 20,
                HousingId = Guid.NewGuid(),
                SpesificationId = Guid.NewGuid(),
                ImageHouseTypeId = Guid.NewGuid()
            };
            _repository.Setup(repo => repo.Save(It.IsAny<HouseType>())).ReturnsAsync(houseType);
            var result = await _service.CreateNewHouseType(houseType);

            Assert.Equal(houseType, result);
        }

        [Fact]
        public async Task Should_ReturnListHouseType_When_GetAllHouseType()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<HouseType, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(houseTypes);

            var result = await _service.GetAllHouseType(1, 5);

            Assert.Equal(houseTypes.Count, result.Content.Count);
        }

        [Fact]
        public async Task Should_ReturnListHouseType_When_SearchByName()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<HouseType, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(houseTypes);
            var name = "Type";
            var result = await _service.SearchByName(name, 1, 5);

            Assert.Equal(houseTypes.Count, result.Content.Count);
        }
    }
}
