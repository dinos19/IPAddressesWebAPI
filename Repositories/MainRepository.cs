using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Data.SqlClient;

namespace IpaddressesWebAPI.Repositories
{

    public class MainRepository
    {
        private IMemoryCache cacheProvider;
        private DataContext dbContext;

        public MainRepository(IMemoryCache cacheProvider, DataContext dbContext)
        {
            this.cacheProvider = cacheProvider;
            this.dbContext = dbContext;

        }

        public async Task<List<IPAddresses>> GetIPListFromCache()
        {
            List<IPAddresses> list = cacheProvider.Get<List<IPAddresses>>("IPADDRESSLIST");

            return list;
        }

        public async void SetIPListFromCache(List<IPAddresses> CachedList)
        {
            cacheProvider.Set("IPADDRESSLIST", CachedList, TimeSpan.FromHours(1));
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
            cacheProvider.Set("IPADDRESSLIST", list, TimeSpan.FromHours(1));
        }

        public async Task<List<ReportModel>> ReportCountries(string parameteres)
        {
            List<ReportModel> tmpList = new List<ReportModel>();
            string[] words = parameteres.Split(",", System.StringSplitOptions.RemoveEmptyEntries); //

            //string connectionstring = "Server=(localdb)\\\\MSSQLLocalDB;Database=superherodb;Trusted_Connection=True;Encrypt=False";
            string connectionstring = @"Server=(localdb)\MSSQLLocalDB;Database=superherodb;Trusted_Connection=True;Encrypt=False;";
            string whereclause = "";
            foreach (string param in words)
            {
                if (whereclause == "")
                    whereclause += $@" where Countries.TwoLetterCode = '{param}'";
                else
                    whereclause += $@" OR Countries.TwoLetterCode = '{param}'";
            }
            FormattableString query = $"select Countries.Name, Count(IPAddresses.CountryId)as AddressesCount, MAX(UpdatedAt) as LastAddressUpdated from IPAddresses inner join Countries  on IPAddresses.CountryId = Countries.Id {whereclause} group by Countries.Name";


            using (SqlConnection connection = new SqlConnection(
                       connectionstring))
            {
                SqlCommand command = new SqlCommand(
                    query.ToString(), connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tmpList.Add(new ReportModel
                        {
                            AddressesCount = (int)reader["AddressesCount"],
                            LastAddressUpdated = (string)reader["LastAddressUpdated"],
                            Name = (string)reader["Name"],
                        });
                    }
                }
            }
            return tmpList;
        }
    }
}

