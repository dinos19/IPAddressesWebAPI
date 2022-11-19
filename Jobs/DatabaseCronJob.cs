using Hangfire;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;

namespace IpaddressesWebAPI.Jobs
{
    public class DatabaseCronJob
    {
        private readonly IPAddressesRepository iPAddressesRepository;
        private readonly CountryRepository countryRepository;
        private readonly MainRepository mainRepository;
        public DatabaseCronJob(IPAddressesRepository iPAddressesRepository, CountryRepository countryRepository, MainRepository mainRepository)
        {
            this.iPAddressesRepository = iPAddressesRepository;
            this.countryRepository = countryRepository;
            this.mainRepository = mainRepository;

        }

        public async Task HandleBatch(List<IPAddresses> batch)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var ipaddress in batch)
            {
                await HandleIP(ipaddress);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"@@ HandleBatch method took {elapsedMs}ms");
        }

        private async Task HandleIP(IPAddresses ipaddres)
        {

            var currentCountry = await countryRepository.GetCountryByID(ipaddres.CountryId);
            var newCountry = await ip2cService.GetFromip2c(ipaddres.IP);

            if (currentCountry.Name.ToLower() != newCountry.Name.ToLower())
            {
                //country changed, //look up the country from db
                var tmpcountry = await countryRepository.GetCountryByTwoLetter(newCountry.TwoLetterCode);
                if (tmpcountry != null)
                {
                    // exists 
                    var tmpaddress = await iPAddressesRepository.UpdateNewCountryAndDate(ipaddres, tmpcountry.Id);
                    mainRepository.AddToIPList(tmpaddress);
                    //inform cache
                }
                else
                {
                    //not exist, add country to db , update date of ipaddress, inform cache
                    var newCountryID = await countryRepository.AddCountry(newCountry);
                    var tmpaddress = await iPAddressesRepository.UpdateNewCountryAndDate(ipaddres, newCountryID);
                    mainRepository.AddToIPList(tmpaddress);
                }

            }
        }
    }
}
