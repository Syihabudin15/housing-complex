using HousingComplex.Dto;
using HousingComplex.DTOs;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HousingComplex.Controllers
{
    [ApiController]
    [Route("api/spesifications")]
    public class SpesificationController : BaseController
    {
        private readonly ISpesificationService _spesificationService;

        public SpesificationController(ISpesificationService spesificationService)
        {
            _spesificationService = spesificationService;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> CreateNewSpesification([FromBody] Spesification spesification)
        {
            var result = await _spesificationService.CreateNewSpesification(spesification);
            CommonResponse<Spesification> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Successfully create new Spesification",
                Data = result
            };
            return Created("spesifications", response);
        }

        [HttpGet]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> GetAllSpesification([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _spesificationService.GetAllSpesification(page, size);
            CommonResponse<PageResponse<Spesification>> response = new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Successfully get all data Spesification",
                Data = result
            };
            return Ok(response);
        }
    }
}
