using Eshopper_website.Services.GHN;
using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Areas.Admin.Controllers.Api
{
    [Area("Admin")]
    [Route("api/ghn/address")]
    [ApiController]
    public class GHNAddressController : Controller
    {
        private readonly IGHNService _gHNService;
        public GHNAddressController(IGHNService gHNService)
        {
            _gHNService = gHNService;
        }

        [HttpGet("province")]
        public async Task<IActionResult> GetProvinceAsync()
        {
            try
            {
                var response = await _gHNService.GetProvinceAsync();
                return Ok(response);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("district/{province_id}")]
        public async Task<IActionResult> GetDistrictAsync(int province_id)
        {
            try
            {
                var response = await _gHNService.GetDistrictAsync(province_id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("ward/{district_id}")]
        public async Task<IActionResult> GetWardAsync(int district_id)
        {
            try
            {
                var response = await _gHNService.GetWardAsync(district_id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
