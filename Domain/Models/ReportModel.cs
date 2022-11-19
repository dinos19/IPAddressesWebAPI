namespace IpaddressesWebAPI.Models
{
    public class ReportModel
    {
        public string Name { get; set; } = string.Empty;
        public int AddressesCount { get; set; } = 0;
        public string LastAddressUpdated { get; set; } = string.Empty;
    }
}
