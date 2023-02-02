using HousingComplex.Dto.Housing;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using HousingComplex.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestHousingComplex.Services
{
    public class HousingServiceTest
    {
        private readonly Mock<IRepository<Housing>> _repository;
        private readonly Mock<IPersistence> _persistence;
        private readonly IHousingService _service;
        private readonly List<Housing> housings = new List<Housing>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "old Grand",
                Address = "jl. juanda",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend",
                Developer = new()
                {
                    Id = Guid.NewGuid(),
                    Name = "oke",
                    Address = "okelagi",
                    PhoneNumber = "1234567890",
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "new Grand",
                Address = "jl. budiman",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend",
                Developer = new()
                {
                    Id = Guid.NewGuid(),
                    Name = "oke",
                    Address = "okelagi",
                    PhoneNumber = "1234567890",
                }
            }
        };

        public HousingServiceTest()
        {
            _repository = new Mock<IRepository<Housing>>();
            _persistence = new Mock<IPersistence>();
            _service = new HousingService(_repository.Object, _persistence.Object);
        }

        [Fact]
        public async Task Should_ReturnHousing_When_CreateHousing()
        {
            var housing = new Housing
            {
                Id = Guid.NewGuid(),
                Name = "Granded",
                Address = "jl. juanda",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend"
            };

            _repository.Setup(repo => repo.Save(It.IsAny<Housing>())).ReturnsAsync(housing);
            _persistence.Setup(pers => pers.SaveChangesAsync());

            var result = await _service.CreateNewHousing(housing);

            Assert.Equal(housing, result);
        }

        [Fact]
        public async Task Should_ReturnHousing_When_GetById()
        {
            var housing = new Housing
            {
                Id = Guid.NewGuid(),
                Name = "Granded",
                Address = "jl. juanda",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend"
            };
            _repository.Setup(repo => repo.FindById(It.IsAny<Guid>())).ReturnsAsync(housing);

            var result = await _service.GetById(housing.Id.ToString());

            Assert.Equal(housing, result);
        }

        [Fact]
        public async Task Should_ReturnListHousingResponse_When_GetAllHousing()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<Housing, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(housings);
            PageResponse<HousingResponse> pageResponse = new PageResponse<HousingResponse>
            {
                Content = housings.Select(h => new HousingResponse
                {
                    Id = h.Id.ToString(),
                    Name = h.Name,
                    Address = h.Address,
                    City = h.City,
                    OpenTime = h.OpenTime
                }).ToList(),
                TotalPages = 1,
                TotalElement = 2
            };

            var result = await _service.GetAllHousing(1, 5);
            
            Assert.Equal(pageResponse.Content.Count, result.Content.Count);
        }

        [Fact]
        public async Task Should_ReturnListHousing_When_SearchByName()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<Housing, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(housings);
            PageResponse<HousingResponse> pageResponse = new PageResponse<HousingResponse>
            {
                Content = housings.Select(h => new HousingResponse
                {
                    Id = h.Id.ToString(),
                    Name = h.Name,
                    Address = h.Address,
                    City = h.City,
                    OpenTime = h.OpenTime
                }).ToList(),
                TotalPages = 1,
                TotalElement = 2
            };

            var result = await _service.SearchByName("Grand", 1, 5);

            Assert.Equal(pageResponse.Content.Count, result.Content.Count);
        }

        [Fact]
        public async Task Should_ReturnListHousing_When_SearchByCity()
        {
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<Housing, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string[]>()
        )).ReturnsAsync(housings);
            PageResponse<HousingResponse> pageResponse = new PageResponse<HousingResponse>
            {
                Content = housings.Select(h => new HousingResponse
                {
                    Id = h.Id.ToString(),
                    Name = h.Name,
                    Address = h.Address,
                    City = h.City,
                    OpenTime = h.OpenTime
                }).ToList(),
                TotalPages = 1,
                TotalElement = 2
            };

            var result = await _service.SearchByCity("Bandung", 1, 5);

            Assert.Equal(pageResponse.Content.Count, result.Content.Count);
        }
    }
}
