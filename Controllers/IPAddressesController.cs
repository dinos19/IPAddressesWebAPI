using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IpaddressesWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class IPAddressesController : ControllerBase
    {            

        private readonly DataContext context;
        private readonly IPAddressesRepository iPAddressesRepository;

        public IPAddressesController(DataContext context, IPAddressesRepository iPAddressesRepository)
        {
            this.context = context;
            this.iPAddressesRepository = iPAddressesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<IPAddresses>>> GetIPAddresses()
        {
            var addresses = await iPAddressesRepository.GetIPAddresses();
            return Ok(addresses);

        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<IPAddresses>> GetIPAddressyByID(int id)
        //{
        //    IPAddresses iPAddress = null;
        //    iPAddress = await context.IPAddresses.FindAsync(id);
        //    return Ok(iPAddress);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddIPAddress(IPAddresses ipaddress)
        //{
        //    if (!IpHelper.isIP(ipaddress.IP))
        //        return BadRequest("Ip is not valid");
        //    await iPAddressesRepository.InsertIPAddressOrUpdateDate(ipaddress);
        //    return Ok();
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateIPAddress(IPAddresses request)
        //{
        //    var country = await context.IPAddresses.FindAsync(request.Id);
        //    if (country == null)
        //        return NotFound("Country not found");

        //    //TODO-upodate date
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}
        //[HttpDelete]
        //public async Task<IActionResult> DeleteIPAddress(int id)
        //{
        //    var returnedID = await iPAddressesRepository.DeleteIPAddress(id);
        //    if (returnedID < 0)
        //        return NotFound("IP not found");
        //    return Ok(returnedID);
        //}
    }
}
