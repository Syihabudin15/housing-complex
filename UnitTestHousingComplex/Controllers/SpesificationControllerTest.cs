using HousingComplex.Controllers;
using HousingComplex.Dto;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestHousingComplex.Controllers
{
    public class SpesificationControllerTest
    {
        private readonly Mock<ISpesificationService> _service;
        private readonly SpesificationController _controller;
        public SpesificationControllerTest()
        {
            _service = new Mock<ISpesificationService>();
            _controller = new SpesificationController(_service.Object);
        }

        [Fact]
        public async Task Should_ReturnCreated_When_CreateSpesification()
        {
            var specsification = new Spesification
            {
                Id = Guid.NewGuid(),
                Bedrooms = 2,
                Bathrooms = 1,
                Kitchens = 1,
                Carport = true,
                SwimmingPool = true,
                SecondFloor = false
            };
            CommonResponse<Spesification> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Successfully create new Spesification",
                Data = specsification
            };
            _service.Setup(serv => serv.CreateNewSpesification(It.IsAny<Spesification>())).ReturnsAsync(specsification);

            var controller = await _controller.CreateNewSpesification(specsification) as CreatedResult;
            var result = controller?.Value as CommonResponse<Spesification>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }

        [Fact]
        public async Task Should_ReturnOk_When_GetAllSpesification()
        {
            List<Spesification> spesifications = new List<Spesification>
            {
                new()
                {
                Id = Guid.NewGuid(),
                Bedrooms = 2,
                Bathrooms = 1,
                Kitchens = 1,
                Carport = true,
                SwimmingPool = true,
                SecondFloor = false
                },
                new()
                {
                Id = Guid.NewGuid(),
                Bedrooms = 4,
                Bathrooms = 2,
                Kitchens = 2,
                Carport = true,
                SwimmingPool = true,
                SecondFloor = true
                }
            };
            var totalPage = (int)Math.Ceiling(spesifications.Count() / (decimal)5);
            CommonResponse<PageResponse<Spesification>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Spesification",
                Data = new PageResponse<Spesification>
                {
                    Content = spesifications,
                    TotalPages = totalPage,
                    TotalElement = spesifications.Count
                }
            };
            _service.Setup(serv => serv.GetAllSpesification(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(response.Data);

            var controller = await _controller.GetAllSpesification(1, 5) as OkObjectResult;
            var result = controller?.Value as CommonResponse<PageResponse<Spesification>>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }
    }
}
