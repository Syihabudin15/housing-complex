using HousingComplex.Dto.HouseType;
using HousingComplex.DTOs;
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
                ImageHouseTypeId = Guid.NewGuid(),
                Spesification = new()
                {
                    Id = Guid.NewGuid(),
                    Bedrooms = 1,
                    Bathrooms = 2,
                    Kitchens = 1,
                    Carport = true,
                    SwimmingPool = false,
                    SecondFloor = false
                },
                Housing = new()
                {
                    Id = Guid.NewGuid(),
                    Name = "okeOke",
                    Address = "jl. Kenanga",
                    City = "jakarta",
                    DeveloperId = Guid.NewGuid(),
                    OpenTime = "weekend",
                    Developer = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Syihab",
                        Address = "kp. Cantel",
                        PhoneNumber = "0912873421443",
                        UserCredentialId = Guid.NewGuid(),
                    }
                },
                ImageHouseType = new()
                {
                    Id = Guid.NewGuid(),
                    FileName = "success.png",
                    FileSize = 2121,
                    FilePath = "Resource/Image/success.png",
                    ContentType = "image/png"
                }
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
        public async Task Should_ReturnHouseType_When_GetById()
        {
            var houseType = new HouseType()
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
            _repository.Setup(repo => repo.Find(It.IsAny<Expression<Func<HouseType, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(houseType);

            var result = await _service.GetForTransaction(houseType.Id.ToString());

            Assert.Equal(houseType, result);
        }

        [Fact]
        public async Task Should_ReturnListHouseTypeResponse_When_GetAllHouseType()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<HouseType, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(houseTypes);
            PageResponse<HouseTypeResponse> pageResponse = new()
            {
                Content = houseTypes.Select(h => new HouseTypeResponse
                {
                    Id = h.Id.ToString(),
                    Name = h.Name,
                    Description = h.Description,
                    Price = h.Price,
                    StockUnit = h.StockUnit
                }).ToList(),
                TotalPages = 1,
                TotalElement = 2
            };


            var result = await _service.GetAllHouseType(1, 5);

            Assert.Equal(pageResponse.Content.Count, result.Content.Count);
        }

        [Fact]
        public async Task Should_ReturnListHouseTypeResponse_When_SearchByName()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<HouseType, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(houseTypes);
            PageResponse<HouseTypeResponse> pageResponse = new()
            {
                Content = houseTypes.Select(h => new HouseTypeResponse
                {
                    Id = h.Id.ToString(),
                    Name = h.Name,
                    Description = h.Description,
                    Price = h.Price,
                    StockUnit = h.StockUnit,
                    Spesification = h.Spesification
                }).ToList(),
                TotalPages = 1,
                TotalElement = 2
            };
            var name = "30/70";

            var result = await _service.SearchByName(name, 1, 5);

            Assert.Equal(pageResponse.Content.Count, result.Content.Count);
        }
    }
}
