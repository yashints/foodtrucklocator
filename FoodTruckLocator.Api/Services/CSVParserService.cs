using FoodTruckLocator.Api.Model;
using FoodTruckLocator.Api.Models;
using FoodTruckLocator.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace FoodTruckLocator.Api.Services
{
    public class CSVParserService : ICSVParserService
    {
        private readonly IMemoryCache _memoryCache;
        public CSVParserService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IEnumerable<MobileFoodFacility> ReadAll()
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new CsvMobileFoodFacilityMapping();
            var csvParser = new CsvParser<MobileFoodFacility>(csvParserOptions, csvMapper);
            var result = csvParser
                         .ReadFromFile(@"Data/Mobile_Food_Facility_Permit.csv", Encoding.ASCII)
                         .Select(x => x.Result)
                         .ToList();
            _memoryCache.Set(Keys.FoodTruckKey, result);
            return result;
        }
    }
}
