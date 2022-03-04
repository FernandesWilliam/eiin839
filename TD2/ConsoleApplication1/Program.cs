using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        // addresse du serveur local 
        private static string url = "http://localhost:8080/Incr?param1=";

        public class Incr
        {
            public int incr
            {
                get;
                set;

            }
        }
        public static async Task Main()
        {

            HttpClient client = new HttpClient();
            // changer ici la valeur de incrValue 
            int incrValue = 3; 
            var response = client.GetAsync(url+incrValue);
            var responseBody = response.Result.Content.ReadAsStringAsync().Result;

            Console.Write(Json.JsonParser.Deserialize<Incr>(responseBody).incr);

        }
    }
}