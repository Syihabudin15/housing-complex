using HousingComplex.Dto;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HousingComplex.Controllers
{
    [Route("api/house-types")]
    public class HouseTypeController : BaseController
    {
        private readonly IHouseTypeService _houseTypeService;

        public HouseTypeController(IHouseTypeService houseTypeService)
        {
            _houseTypeService = houseTypeService;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> CreateNewHouseType([FromBody] HouseType request)
        {
            var result = await _houseTypeService.CreateNewHouseType(request);
            CommonResponse<HouseType> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Succesfully create Housetype",
                Data = result
            };
            return Created("house-type", response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHouseType([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _houseTypeService.GetAllHouseType(page, size);
            CommonResponse<PageResponse<HouseType>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Housetype",
                Data = result
            };
            return Ok(response);
        }
    }
}
