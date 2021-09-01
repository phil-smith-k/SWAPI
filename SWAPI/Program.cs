using SWAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWAPI
{
    class Program
    {
        // HTTP Requests: 
        // 1. HttpMethod / URL - what we're doing when we get there - where we are sending the request 
        // Common HttpMethods :  POST (create), GET (read), PUT (update), DELETE (delete)
        // 2. Headers - metadata about the request itself KVP - Key, Value Pairs
        // 3. Body - holds the data we want to send (payload) parameters (JSON)

        // HTTP Response: 
        // 1. StatusCode - a specific code of what happened when the request was processed
        // 2. Headers - they might be different from the request
        // 3. Body - the data returned to us from the web server ( API )

        // Deserialization - turning a string into an object, JSON to C#
        // Serialization - the process of turning an object to a string, C# to JSON

        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

                                           // Builds the GET request AND sends it to the url
            HttpResponseMessage response = await httpClient.GetAsync("https://swapi.dev/api/people/1");

            if (response.IsSuccessStatusCode)
            {
                // This is where we deserialize the body of the response
                Person luke = response.Content.ReadAsAsync<Person>().Result;
                Console.WriteLine(luke.Name);

                foreach (var url in luke.Vehicles)
                {
                    HttpResponseMessage vehicleResponse = httpClient.GetAsync(url).Result;
                    // This is where we deserialize the body of the response
                    Vehicle vehicle = vehicleResponse.Content.ReadAsAsync<Vehicle>().Result;

                    Console.WriteLine(vehicle.Name);
                }
            }

            SWAPIService service = new SWAPIService();
                                                                                    // We can also use await (instead of .Result), if we are in an async method
            Person person = service.GetPersonAsync("https://swapi.dev/api/people/11").Result;

            if (person != null)
            {
                Console.WriteLine(person.Name);

                foreach (var url in person.Vehicles)
                {
                    var vehicle = service.GetVehicleAsync(url).Result;
                    if (vehicle != null)
                    {
                        Console.WriteLine(vehicle.Name);
                    }
                }
            }

            var genericResponse = service.GetAsync<Vehicle>("https://swapi.dev/api/vehicles/4").Result;
            if (genericResponse != null)
            {
                Console.WriteLine(genericResponse.Name);
            }
            else
            {
                Console.WriteLine("Targeted object not found");
            }

            SearchResult<Person> results = service.GetPersonSearchAsync("skywalker").Result;
            foreach(Person p in results.Results)
            {
                Console.WriteLine(p.Name);
            }

            var genericSearch = service.GetSearchAsync<Vehicle>("speeder", "vehicles").Result;
            var genericVehicleSearch = service.GetVehicleSearchAsync("speeder").Result;

            Console.ReadLine();
        }
    }
}
