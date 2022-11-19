using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Models;
using Microsoft.Extensions.Caching.Memory;


namespace IpaddressesWebAPI.Repositories
{

    public class MainRepository
    {
        private IMemoryCache cacheProvider;

        public MainRepository(IMemoryCache cacheProvider)
        {
            this.cacheProvider = cacheProvider;

        }

        public async Task<List<IPAddresses>> GetIPListFromCache()
        {
            List<IPAddresses> list = cacheProvider.Get<List<IPAddresses>>("IPADDRESSLIST");

            return list;
        }

        public async void AddToIPList(IPAddresses CachedAddress)
        {
            if (CachedAddress == null) return;
            List<IPAddresses> list = cacheProvider.Get<List<IPAddresses>>("IPADDRESSLIST");
            if (list != null)
            {
                //lookup for the address, if not exist then add it
                bool containsItem = list.Any(item => item.IP == CachedAddress.IP);

                if (!containsItem)
                    list.Add(CachedAddress);
            }
            else
                list = new List<IPAddresses>
                {
                    CachedAddress
                };
            cacheProvider.Set("IPADDRESSLIST", list);
        }

        
    }
}

