using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IpaddressesWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly CountryRepository countryRepository;

        public CountriesController(DataContext dataContext,CountryRepository countryRepository)
        {
            this.dataContext = dataContext;
            this.countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Countries>>> GetCountries()
        {
            return Ok(await countryRepository.GetCountries());

        }
        [HttpGet("{id}")]

        public async Task<ActionResult<Countries>> GetCountryByID(int id)
        {
            return Ok(await countryRepository.GetCountryByID(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(Countries country)
        {
            
            await countryRepository.AddCountry(country);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatedCountry(Countries request)
        {
             countryRepository.UpdatedCountry(request);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            countryRepository.DeleteCountryById(id);
            return Ok();
        }
    }
}
