using Microsoft.EntityFrameworkCore;

namespace IpaddressesWebAPI.Models
{
    [Index("IP", "CountryId", IsUnique = true, Name = "IX_IP_COUNTRYID")]
    public class IPAddresses
    {
        public int Id { get; set; }

        public string IP { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
