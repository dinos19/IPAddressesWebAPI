using IpaddressesWebAPI.Handlers;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IpaddressesWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly MainHandler mainHandler;

        public MainController(MainHandler mainHandler)
        {
            this.mainHandler = mainHandler;
        }

        [HttpGet]
        public async Task<IActionResult> CheckMyIP(string ip)
        {
            if (!IpHelper.isIP(ip))
                return BadRequest("Not valid IP");
            Countries country = await mainHandler.HandleNewRequest(ip);
            if (country == null)
                return NotFound("Country not found");
            return Ok(country);
        }

        [HttpGet("{stringList}")]
        public async Task<IActionResult> ReportCountry(List<string> stringList)
        {
            List<ReportModel> countries = await mainHandler.ReportCountries(stringList[0]);
            return Ok(countries);
        }
    }
}
