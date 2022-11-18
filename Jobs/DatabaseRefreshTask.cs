using Hangfire;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;

namespace IpaddressesWebAPI.Jobs
{
    public class DatabaseRefreshTask
    {
        public int index;
        private readonly IPAddressesRepository iPAddressesRepository;
        private readonly RecurringJobManager recurringJobManager;
        private readonly CountryRepository countryRepository;
        private readonly MainRepository mainRepository;
        public DatabaseRefreshTask(IPAddressesRepository iPAddressesRepository, RecurringJobManager recurringJobManager, CountryRepository countryRepository, MainRepository mainRepository)
        {
            this.iPAddressesRepository = iPAddressesRepository;
            this.recurringJobManager = recurringJobManager;
            this.countryRepository = countryRepository;
            this.mainRepository = mainRepository;
            index = 0;

        }

        public async Task InsertTask()
        {
            recurringJobManager.AddOrUpdate(
    "run every 1 hour",
    () => UpdateDBJob(),
    "0 * * * *");
        }


        public async Task UpdateDBJob()
        {
            Console.WriteLine($"Job started at {DateTime.Now}");
            //get 100 of items updated time < mpw -1 min
            var iPAddresses = await iPAddressesRepository.GetIPAddressesToUpdate(index);
            //check if country changed
            do
            {
                foreach (IPAddresses ipaddres in iPAddresses)
                {
                    //check on service
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
                    else
                    {
                        //nothing changed, update date of ipaddress
                        await iPAddressesRepository.UpdateDate(ipaddres);
                        //we try to inform cache anyway to make our search easier later
                        mainRepository.AddToIPList(ipaddres);

                    }
                }
                index++;
                iPAddresses = await iPAddressesRepository.GetIPAddressesToUpdate(index, 5);
            } while (iPAddresses != null && iPAddresses.Count > 0);

        }
    }
}
