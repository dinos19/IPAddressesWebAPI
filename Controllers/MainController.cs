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

        [HttpPost]
        
        public async Task<IActionResult> CreateData()
        {
            //it takes about 50 seconds if 511 ip are all new
            string StartIP = "152.89.40.0";
            int IPCount = 511;

            uint n = DummyHelper.ParseIP(StartIP);
            string[] range = new string[IPCount];
            for (uint i = 0; i < IPCount; i++)
            {
                var tmpip = DummyHelper.FormatIP(n + i);
                await mainHandler.HandleNewRequest(tmpip);
            }


            return Ok();
        }
    }
}
