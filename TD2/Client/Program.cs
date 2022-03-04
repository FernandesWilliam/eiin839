
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static string url = "http://localhost:8080/Incr?param1=1";

        public static async Task Main()
        {

            HttpClient client = new HttpClient();
            var response = client.GetAsync(url);
            var responseBody = await response.Result.Content.ReadAsStreamAsync();
            Console.Write(responseBody);

        }
    }

}

