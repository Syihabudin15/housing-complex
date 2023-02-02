using HousingComplex.Dto;
using HousingComplex.Dto.ImageType;
using HousingComplex.Entities;
using HousingComplex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HousingComplex.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageHouseTypeController : BaseController
    {
        private readonly IImageHouseTypeService _imageHouseTypeService;

        public ImageHouseTypeController(IImageHouseTypeService imageHouseTypeService)
        {
            _imageHouseTypeService = imageHouseTypeService;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> CreateNewImageHouseType([FromForm] IFormFile file)
        {
            var result = await _imageHouseTypeService.CreateNewImageHouseType(file);
            CommonResponse<UploadFileResponse> response = new()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Successfully created new Image House Type",
                Data = result
            };
            return Created("images", response);
        }
        
        [AllowAnonymous]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var profilePicture = await _imageHouseTypeService.DownloadProfilePicture(id);
            return File(profilePicture.MemoryStream, profilePicture.ContentType, profilePicture.Filename);
        }

    }
}
