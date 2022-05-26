using FoodTruckLocator.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FoodTruckLocator.Web.Data
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly string _apiURL;
        private readonly TokenProvider _tokenProvider;
        private IEnumerable<MobileFoodFacility> _nearbyTrucks = Array.Empty<MobileFoodFacility>();
        public HttpClientService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            TokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _apiURL = configuration["DownstreamApi:BaseUrl"];
            _tokenProvider = tokenProvider;
        }

        public async Task<IEnumerable<MobileFoodFacility>> GetNearbyTrucksAsync(double latitude, double longitude)
        {
            //var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "Trucks.Read" });
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_apiURL}/foodtrucklocator/nearby?latitude={latitude}&longitude={longitude}");

            request.Headers.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.AccessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                _nearbyTrucks = JsonConvert.DeserializeObject<IEnumerable<MobileFoodFacility>>(responseStream);
                return _nearbyTrucks;
            }
            else
            {
                throw new Exception($"API call failed with {response.StatusCode}");
            }

        }
    }
}
