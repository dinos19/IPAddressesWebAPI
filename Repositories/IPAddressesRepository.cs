using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IpaddressesWebAPI.Repositories
{
    public class IPAddressesRepository
    {
        private readonly DataContext dataContext;

        public IPAddressesRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<int> InsertIPAddressOrUpdateDate(IPAddresses ipaddress)
        {
            
            ipaddress.CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            ipaddress.UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            IPAddresses tmpip = await GetIPAddress(ipaddress.IP);
            if (tmpip == null)
            {
                //INSERT
                dataContext.IPAddresses.Add(ipaddress);
                await dataContext.SaveChangesAsync();
            }
            else
            {
                //UPDATE
                tmpip.CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                await dataContext.SaveChangesAsync();

            }


            return ipaddress.Id;
        }
        public async Task<List<IPAddresses>> GetIPAddresses()
        {
            return await dataContext.IPAddresses.ToListAsync();

        }
        public async Task<IPAddresses> GetIPAddress(string ip)
        {
            IPAddresses iPAddress;
            iPAddress = dataContext.IPAddresses
                .Where(tmpip => tmpip.IP == ip)
                .FirstOrDefault();
            return iPAddress;
        }

        public async Task<int> DeleteIPAddress(int id)
        {
            var ipaddress = await dataContext.IPAddresses.FindAsync(id);
            if (ipaddress == null)
                return -1;

            dataContext.IPAddresses.Remove(ipaddress);
            await dataContext.SaveChangesAsync();
            return ipaddress.Id;
        }

        

        internal async Task<IPAddresses> UpdateNewCountryAndDate(IPAddresses ipaddres, int countryid)
        {
            ipaddres.UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            ipaddres.CountryId = countryid;
            await dataContext.SaveChangesAsync();
            return ipaddres;
        }

        //put
        //delete
    }
}

