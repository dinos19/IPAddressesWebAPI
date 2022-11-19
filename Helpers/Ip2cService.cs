using IpaddressesWebAPI.Models;

namespace IpaddressesWebAPI.HelperClasses
{
    public class ip2cService
    {
        public static async Task<Countries> GetFromip2c(string ip)
        {
            Countries country = null;
            //https://localhost:7166/Test/CheckMyIP/193.92.212.96    1;GR;GRC;Greece
            string textToReturn = "";
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"http://ip2c.org/{ip}"))
                {
                    try
                    {
                        var response = await httpClient.SendAsync(request);
                        textToReturn = await response.Content.ReadAsStringAsync();
                        string[] words = textToReturn.Split(";", System.StringSplitOptions.RemoveEmptyEntries); //
                        country = new Countries
                        {
                            TwoLetterCode = words[1],
                            ThreeLetterCode = words[2],
                            Name = words[3],
                        };
                        //response.EnsureSuccessStatusCode();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            return country;
        }
    }
}
