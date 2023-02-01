using HousingComplex.Controllers;
using HousingComplex.Dto;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Repositories;
using HousingComplex.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestHousingComplex.Controllers
{
    public class HousingControllerTest
    {
        private readonly Mock<IHousingService> _service;
        private readonly HousingController _controller;
        public HousingControllerTest()
        {
            _service = new Mock<IHousingService>();
            _controller = new HousingController(_service.Object);
        }

        [Fact]
        public async Task Should_ReturnCreated_When_CreateHousing()
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
            CommonResponse<Housing> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Succesfully create new Housing",
                Data = housing
            };
            _service.Setup(serv => serv.CreateNewHousing(It.IsAny<Housing>())).ReturnsAsync(housing);
            
            var controller = await _controller.CreateNewHousing(housing) as CreatedResult;
            var result = controller?.Value as CommonResponse<Housing>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }

        [Fact]
        public async Task Should_ReturnOk_When_GetAllHousing()
        {
            List<Housing> housings = new List<Housing>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                Name = "old Grand",
                Address = "jl. juanda",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                Name = "new Grand",
                Address = "jl. budiman",
                City = "Bandung",
                DeveloperId = Guid.NewGuid(),
                OpenTime = "Weekend"
                }
            };
            var page = 1;
            var size = 5;
            var totalPage = (int)Math.Ceiling(housings.Count() / (decimal)size);

            var response = new CommonResponse<PageResponse<Housing>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Housing",
                Data = new PageResponse<Housing>
                {
                    Content = housings,
                    TotalPages = totalPage,
                    TotalElement = housings.Count()
                }
            };
            _service.Setup(serv => serv.GetAllHousing(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(response.Data);

            var controller = await _controller.GetAllHousing(page, size) as OkObjectResult;
            var result = controller?.Value as CommonResponse<PageResponse<Housing>>;
            
            _service.Verify(serv => serv.GetAllHousing(1, 5), Times.Once);
            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Data.Content.Count, result?.Data.Content.Count);
            Assert.Equal(response.Message, result?.Message);
        }
    }
}
