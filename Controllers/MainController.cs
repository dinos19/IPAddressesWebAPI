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

        [HttpGet("{ip}")]

        public async Task<IActionResult> CheckMyIP(string ip)
        {
            if (!IpHelper.isIP(ip))
                return BadRequest("Not valid IP");
            Countries country = await mainHandler.HandleNewRequest(ip);
            if (country == null)
                return NotFound("Country not found");
            return Ok(country);
        }

        [HttpGet]
        [Route("api/[controller]/ReportCountry")]

        public async Task<IActionResult> ReportCountry(List<string> stringList)
        {
            string param = (stringList.Count == 0) ? "" : stringList[0];
            List <ReportModel> countries = await mainHandler.ReportCountries(param);
            return Ok(countries);
        }

        [HttpPost]
        [Route("api/[controller]/CreateData")]

        public async Task<IActionResult> CreateData()
        {
            //feed db
            //it takes about 50 seconds if 511 ip are all new
            string StartIP = "152.89.40.0";
            int IPCount = 200;

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
