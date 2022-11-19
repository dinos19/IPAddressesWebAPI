using Hangfire;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;

namespace IpaddressesWebAPI.Handlers
{
    public class MainHandler
    {
        List<IPAddresses> iPAddresses { get; set; } = new List<IPAddresses>();
        private readonly MainRepository mainRepository;
        private readonly IPAddressesRepository iPAddressesRepository;
        private readonly CountryRepository countryRepository;
        private readonly RecurringJobManager recurringJobManager;

        public MainHandler(MainRepository mainRepository, IPAddressesRepository iPAddressesRepository, CountryRepository countryRepository, RecurringJobManager recurringJobManager)
        {
            this.mainRepository = mainRepository;
            this.iPAddressesRepository = iPAddressesRepository;
            this.countryRepository = countryRepository;
            this.recurringJobManager = recurringJobManager;

        }

        public async Task<Countries> HandleNewRequest(string ip)
        {
            //check in memory
            IPAddresses tmpIP = null;
            Countries newCountry = null;
            Countries currentCountry = null;
            iPAddresses = await mainRepository.GetIPListFromCache();
            if (iPAddresses != null && iPAddresses.Count > 0)
            {
                foreach (var item in iPAddresses)
                {
                    if (item.IP == ip)
                    {
                        await iPAddressesRepository.InsertIPAddressOrUpdateDate(item); //update date
                        tmpIP = item;
                        break;
                    }
                }
            }
            if (iPAddresses == null)
            {
                //search on db
                tmpIP = await iPAddressesRepository.GetIPAddress(ip);
                if (tmpIP != null)
                {
                    await iPAddressesRepository.InsertIPAddressOrUpdateDate(tmpIP);  //update date
                    mainRepository.AddToIPList(tmpIP);
                }
            }

            if (tmpIP == null)
            {
                int newcountryid = 0;
                //get from service
                newCountry = await ip2cService.GetFromip2c(ip);
                if (newCountry == null || newCountry.TwoLetterCode == "") return null;
                //check if we have this country
                currentCountry = await countryRepository.GetCountryByTwoLetter(newCountry.TwoLetterCode);
                if (currentCountry == null)
                {
                    newcountryid = await countryRepository.AddCountry(newCountry);
                }
                else
                    newcountryid = currentCountry.Id;


                IPAddresses ipaddress = new IPAddresses
                {
                    CountryId = newcountryid,
                    IP = ip,
                };
                //todo repo should insert in cache
                await iPAddressesRepository.InsertIPAddressOrUpdateDate(ipaddress);
                mainRepository.AddToIPList(ipaddress);
                newCountry.Id = newcountryid;

            }
            else
            {
                newCountry = await countryRepository.GetCountryByID(tmpIP.CountryId);
            }
            return newCountry;
        }






        public async Task<List<ReportModel>> ReportCountries(string param)
        {

            List<ReportModel> tmpList = await countryRepository.ReportCountries(param);

            return tmpList;
        }
    }
}
