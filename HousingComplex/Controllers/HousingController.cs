using HousingComplex.Dto;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HousingComplex.Controllers
{
    [Route("api/housings")]
    public class HousingController : BaseController
    {
        private readonly IHousingService _housingService;

        public HousingController(IHousingService housingService)
        {
            _housingService = housingService;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> CreateNewHousing([FromBody] Housing housing)
        {
            var result = await _housingService.CreateNewHousing(housing);
            CommonResponse<Housing> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Succesfully create new Housing",
                Data = result
            };
            return Created("housing",response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHousing([FromQuery]int page, [FromQuery] int size)
        {
            var result = await _housingService.GetAllHousing(page, size);
            CommonResponse<PageResponse<Housing>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Housing",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("name")]
        public async Task<IActionResult> SearchByName([FromQuery]string name, [FromQuery]int page, [FromQuery]int size)
        {
            var result = await _housingService.SearchByName(name, page, size);
            CommonResponse<PageResponse<Housing>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully search all data with name in Housing",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("city")]
        public async Task<IActionResult> SearchByCity([FromQuery]string city, [FromQuery]int page, [FromQuery]int size)
        {
            var result = await _housingService.SearchByCity(city,page, size);
            CommonResponse<PageResponse<Housing>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully search all data with name in Housing",
                Data = result
            };
            return Ok(response);
        }
    }
}
