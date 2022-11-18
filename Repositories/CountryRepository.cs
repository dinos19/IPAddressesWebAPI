using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IpaddressesWebAPI.Repositories
{
    public class CountryRepository
    {
        private readonly DataContext dataContext;

        public CountryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Countries>> GetCountries()
        {
           return await dataContext.Countries.ToListAsync();
        }

        public async Task<Countries> GetCountryByID(int id)
        {
            Countries country = null;
            country = await dataContext.Countries.FindAsync(id);
            return country;
        }

        public async Task<int> AddCountry(Countries country)
        {
            int countryid = 0;
            Countries tmpCountry = await GetCountryByTwoLetter(country.TwoLetterCode);
            if (tmpCountry == null)
            {
                //INSERT
                country.CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                dataContext.Countries.Add(country);
                await dataContext.SaveChangesAsync();
                countryid = country.Id;
            }
            else
                countryid = tmpCountry.Id;
            return countryid;
        }

        public async Task<Countries> GetCountryByTwoLetter(string key)
        {
            Countries country;
            country = dataContext.Countries
                .Where(tmpCountry => tmpCountry.TwoLetterCode.ToLower() == key.ToLower())
                .FirstOrDefault();
            return country;
        }



        public async void UpdatedCountry(Countries request)
        {
            var country = await dataContext.Countries.FindAsync(request.Id);
            if (country == null)
                return;

            country.Name = request.Name;
            country.ThreeLetterCode = request.ThreeLetterCode;
            country.TwoLetterCode = request.TwoLetterCode;
            await dataContext.SaveChangesAsync();
        }

        public async void DeleteCountryById(int id)
        {
            var country = await dataContext.Countries.FindAsync(id);
            if (country == null)
                return ;

            dataContext.Countries.Remove(country);
            await dataContext.SaveChangesAsync();
        }

        
    }
}
