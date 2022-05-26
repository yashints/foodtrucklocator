using FoodTruckLocator.Api.Model;
using FoodTruckLocator.Model;
using GeoCoordinatePortable;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckLocator.Api.Services
{
    public class FoodTruckLocatorService : IFoodTruckLocatorService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICSVParserService _csvParser;
        public FoodTruckLocatorService(IMemoryCache memoryCache, ICSVParserService csvParser)
        {
            _memoryCache = memoryCache;
            _csvParser = csvParser;
        }
        public Task<IEnumerable<MobileFoodFacility>> LocateNearbyTrucksAsync(double latitude, double longitude)
        {
            var coordinate = new GeoCoordinate(latitude, longitude);
            IEnumerable<MobileFoodFacility> foodTrucks = null;

            if (_memoryCache.TryGetValue(Keys.FoodTruckKey, out foodTrucks))
            {

            }
            else
            {
                foodTrucks = _csvParser.ReadAll();
            }

            var result = foodTrucks
                .Where(t => t.FacilityType == "Truck")
                .OrderBy(x => coordinate.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)))
                .Take(5);

            return Task.FromResult(result);
        }

        public Task<IEnumerable<MobileFoodFacility>> ReadAllAsync()
        {
            IEnumerable<MobileFoodFacility> foodTrucks = null;

            if (_memoryCache.TryGetValue(Keys.FoodTruckKey, out foodTrucks))
            {
                return Task.FromResult(foodTrucks);
            }

            return Task.FromResult(_csvParser.ReadAll());
        }
    }
}
