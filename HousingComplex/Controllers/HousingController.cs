﻿using HousingComplex.Dto;
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHousing([FromQuery]int page, [FromQuery] int size)
        {
            var result = await _housingService.GetAllHousing(page, size);
            CommonResponse<PageResponse<Housing>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Housing",
                Data = result
            };
            return Ok(result);
        }
    }
}
