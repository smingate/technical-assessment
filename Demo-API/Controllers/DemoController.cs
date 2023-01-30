using Demo_API.Iservice;
using Demo_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Demo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private ITargetAssetService _targetAssetService;
        public DemoController(ITargetAssetService targetAssetService) : base()
        {
            _targetAssetService = targetAssetService;
        }


        [HttpGet]
        [Route("targetAsset")]
        public async Task<IActionResult> GetTargetAsset()
        {
            try
            {
                var allAssets = await this._targetAssetService.GetTargetAssets();
                if(allAssets.Any())
                {
                    return Ok(allAssets);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
