using FoodTruckLocator.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckLocator.Api.Services
{
    public interface IFoodTruckLocatorService
    {
        Task<IEnumerable<MobileFoodFacility>> ReadAllAsync();
        Task<IEnumerable<MobileFoodFacility>> LocateNearbyTrucksAsync(double latitude, double longitude);
    }
}
