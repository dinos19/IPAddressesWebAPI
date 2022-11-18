using System.Net;

namespace IpaddressesWebAPI.HelperClasses
{
    public static class IpHelper
    {
        public static bool isIP(string ip)
        {
            System.Net.IPAddress ipAddress = null;
            return System.Net.IPAddress.TryParse(ip, out ipAddress);
        }
    }
}
