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
            Countries country = null;
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
                country = await ip2cService.GetFromip2c(ip);
                if (country == null)
                {
                    return null;
                }
                else
                {
                    newcountryid = await countryRepository.AddCountry(country);

                }

                IPAddresses ipaddress = new IPAddresses
                {
                    CountryId = newcountryid,
                    IP = ip,
                };
                //todo repo should insert in cache
                await iPAddressesRepository.InsertIPAddressOrUpdateDate(ipaddress);
                mainRepository.AddToIPList(ipaddress);
                //todo repo should insert in db
            }
            else
            {
                country = await countryRepository.GetCountryByID(tmpIP.CountryId);
            }
            return country;
        }






        public async Task<List<ReportModel>> ReportCountries(string param)
        {

            List<ReportModel> tmpList = await mainRepository.ReportCountries(param);

            return tmpList;
        }
    }
}
