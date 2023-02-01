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
    public class HouseTypeControllerTest
    {
        private readonly Mock<IHouseTypeService> _service;
        private readonly HouseTypeController _controller;
        private readonly List<HouseType> houseTypes = new List<HouseType>
            {
                new()
                {
                Id = Guid.NewGuid(),
                Name = "Type 30/70",
                Description = "new Minimalis Housing with Lates Technologi",
                Price = 200000000,
                StockUnit = 10,
                SpesificationId = Guid.NewGuid(),
                HousingId = Guid.NewGuid(),
                ImageId = Guid.NewGuid()
                },
                new()
                {
                Id = Guid.NewGuid(),
                Name = "Type 36/82",
                Description = "new Minimalis Housing with Lates Technologi",
                Price = 1500000000,
                StockUnit = 20,
                SpesificationId = Guid.NewGuid(),
                HousingId = Guid.NewGuid(),
                ImageId = Guid.NewGuid(),
                }
            };

        public HouseTypeControllerTest()
        {
            _service = new Mock<IHouseTypeService>();
            _controller = new HouseTypeController(_service.Object);
        }

        [Fact]
        public async Task Should_ReturnCreated_When_CreateHouseType()
        {
            var houseType = new HouseType
            {
                Id = Guid.NewGuid(),
                Name = "Type 30/70",
                Description = "new Minimalis Housing with Lates Technologi",
                Price = 200000000,
                StockUnit = 10,
                SpesificationId = Guid.NewGuid(),
                HousingId = Guid.NewGuid(),
                ImageId = Guid.NewGuid()
            };
            CommonResponse<HouseType> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Succesfully create Housetype",
                Data = houseType
            };
            _service.Setup(serv => serv.CreateNewHouseType(It.IsAny<HouseType>())).ReturnsAsync(houseType);

            var controller = await _controller.CreateNewHouseType(houseType) as CreatedResult;
            var result = controller?.Value as CommonResponse<HouseType>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }

        [Fact]
        public async Task Should_ReturnOk_When_GetAllHouseType()
        {
            var totalPage = (int)Math.Ceiling(houseTypes.Count() / (decimal)5);
            CommonResponse<PageResponse<HouseType>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Housetype",
                Data = new PageResponse<HouseType>
                {
                    Content = houseTypes,
                    TotalPages = totalPage,
                    TotalElement = houseTypes.Count
                }
            };
            _service.Setup(serv => serv.GetAllHouseType(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(response.Data);

            var controller = await _controller.GetAllHouseType(1, 5) as OkObjectResult;
            var result = controller?.Value as CommonResponse<PageResponse<HouseType>>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }

        [Fact]
        public async Task Should_ReturnOk_When_SearchByName()
        {
            var name = "30/70";
            var totalPage = (int)Math.Ceiling(houseTypes.Count() / (decimal)5);
            CommonResponse<PageResponse<HouseType>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully search all data with name in HouseType",
                Data = new PageResponse<HouseType>
                {
                    Content = houseTypes,
                    TotalPages = totalPage,
                    TotalElement = houseTypes.Count
                }
            };
            _service.Setup(serv => serv.SearchByName(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(response.Data);

            var controller = await _controller.SearchByName(name, 1, 5) as OkObjectResult;
            var result = controller?.Value as CommonResponse<PageResponse<HouseType>>;

            Assert.Equal(response.StatusCode, result?.StatusCode);
            Assert.Equal(response.Message, result?.Message);
            Assert.Equal(response.Data, result?.Data);
        }
    }
}
