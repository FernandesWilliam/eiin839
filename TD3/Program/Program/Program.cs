// See https://aka.ms/new-console-template for more information


using System.Text.Json;

public class Program
{

    public class Contract
    {
        public String name { get; set; }
        public String commercial_name { get; set; }
        public String country_code { get; set; }
        public List<String> cities { get; set; }

    }

    public class Position
    {
        public float lat { get; set; }
        public float lng { get; set; }

        public double distance(Position position)
        {
            return 0;
        }
    }


    public class Station
    {
        public int number { get; set; }
        public String contract_number { get; set; }
        public String name { get; set; }
        public String address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public int available_bikes { get; set; }
        public string status { get; set; }
        public long last_update { get; set; }


    }
 
    public static async Task Main()
    {

        HttpClient client = new HttpClient();
        
        

        await askForContracts(client);
        await askForOneContracts(client, "amiens");
    }

    public static async Task askForContracts(HttpClient client)
    {
        var response =
            client.GetAsync(
                "https://api.jcdecaux.com/vls/v3/contracts?apiKey=3e29763ee2c2997e780016152e9bd7c5537745d2");
        string responseBody = await response.Result.Content.ReadAsStringAsync();
        var contracts = JsonSerializer.Deserialize<List<Contract>>(responseBody);
    }

    public static async Task<List<Station>> askForOneContracts(HttpClient client, string contract)
    {
        var response =
            client.GetAsync("https://api.jcdecaux.com/vls/v1/stations?contract=" + contract +
                            "&apiKey=3e29763ee2c2997e780016152e9bd7c5537745d2");
        string responseBody = await response.Result.Content.ReadAsStringAsync();
        var stations = JsonSerializer.Deserialize<List<Station>>(responseBody);
        return stations;
    }

    public static Station findNearestStation(Contract contract, Position position)
    {
        HttpClient client = new HttpClient();
        var stations = askForOneContracts(client, contract.name);

        //
        return null;
        //   return stations.Result.Aggregate((curMin,x) => (curMin== null || (x.position.distance(position)?? ); 
        //(curMin == null || (x.DateOfBirth ?? DateTime.MaxValue) <
        // curMin.DateOfBirth ? x : curMin))
    }
}