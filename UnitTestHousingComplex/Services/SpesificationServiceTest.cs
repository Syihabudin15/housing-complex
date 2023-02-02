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
    public class SpesificationServiceTest
    {
        private readonly Mock<IRepository<Spesification>> _repository;
        private readonly Mock<IPersistence> _persistence;
        private readonly ISpesificationService _service;
        public SpesificationServiceTest()
        {
            _repository = new Mock<IRepository<Spesification>>();
            _persistence = new Mock<IPersistence>();
            _service = new SpesificationService(_persistence.Object, _repository.Object);
        }

        [Fact]
        public async Task Should_ReturnSpesification_When_CreateSpesification()
        {
            var spec = new Spesification
            {
                Id = Guid.NewGuid(),
                Bedrooms = 2,
                Bathrooms = 1,
                Kitchens = 1,
                Carport = true,
                SwimmingPool = false,
                SecondFloor = false
            };
            _repository.Setup(repo => repo.Save(It.IsAny<Spesification>())).ReturnsAsync(spec);
            _persistence.Setup(pers => pers.SaveChangesAsync());

            var result = await _service.CreateNewSpesification(spec);

            Assert.Equal(spec, result);
        }

        [Fact]
        public async Task Should_ReturnListSpesification_When_GetAllSpesification()
        {
            List<Spesification> spesifications = new List<Spesification>
            {
                new()
                {
                Id = Guid.NewGuid(),
                Bedrooms = 4,
                Bathrooms = 2,
                Kitchens = 1,
                Carport = true,
                SwimmingPool = false,
                SecondFloor = true
                },
                new()
                {
                Id = Guid.NewGuid(),
                Bedrooms = 2,
                Bathrooms = 1,
                Kitchens = 1,
                Carport = true,
                SwimmingPool = false,
                SecondFloor = false
                }
            };
            _repository.Setup(repository => repository.FindAll(
            It.IsAny<Expression<Func<Spesification, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        )).ReturnsAsync(spesifications);

            var result = await _service.GetAllSpesification(1, 5);

            Assert.Equal(spesifications.Count, result.Content.Count);
        }

    }
}
