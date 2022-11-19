using System.ComponentModel.DataAnnotations.Schema;

namespace IpaddressesWebAPI.Models
{
    public class Countries
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TwoLetterCode { get; set; } = string.Empty;
        public string ThreeLetterCode { get; set; } = string.Empty;
        internal string CreatedAt { get; set; } = string.Empty;
    }
}
