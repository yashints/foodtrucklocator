using FoodTruckLocator.Model;
using System.Collections.Generic;

namespace FoodTruckLocator.Api.Services
{
    public interface ICSVParserService
    {
        IEnumerable<MobileFoodFacility> ReadAll();
    }
}
