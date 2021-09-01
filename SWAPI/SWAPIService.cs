using SWAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWAPI
{
    // Similar to our repository
    public class SWAPIService
    {
        // HttpClient - allows us to send http requests and receive responses
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<Person> GetPersonAsync(string url)
        {
            // sending request / getting response
            var response = await _httpClient.GetAsync(url);

            // making sure it's successful
            if (response.IsSuccessStatusCode)
            {
                // if it is deserialize the content and return the Person
                var person = await response.Content.ReadAsAsync<Person>();
                return person;
            }

            // if not return null
            return null;
        }

        public async Task<Vehicle> GetVehicleAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            return response.IsSuccessStatusCode 
                ? await response.Content.ReadAsAsync<Vehicle>() 
                : null;
        }

                         // making the type variable (we don't know what type we want to return) T instead of a specific type   
        public async Task<T> GetAsync<T>(string url) where T : class
        {
            var response = await _httpClient.GetAsync(url);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<T>()
                : null;
        }

        public async Task<SearchResult<Person>> GetPersonSearchAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://swapi.dev/api/people?search={query}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<SearchResult<Person>>()
                : null;
        }

        public async Task<SearchResult<T>> GetSearchAsync<T>(string query, string category)
        {
            var response = await _httpClient.GetAsync($"https://swapi.dev/api/{category}?search={query}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<SearchResult<T>>()
                : null;
        }

        public async Task<SearchResult<Vehicle>> GetVehicleSearchAsync(string query)
        {
            return await GetSearchAsync<Vehicle>(query, "vehicles");
        }
    }
}
