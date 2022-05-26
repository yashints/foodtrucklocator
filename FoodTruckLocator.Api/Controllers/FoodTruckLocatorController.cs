using FoodTruckLocator.Api.Services;
using FoodTruckLocator.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckLocator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodTruckLocatorController : ControllerBase
    {
        private readonly ILogger<FoodTruckLocatorController> _logger;
        private readonly IFoodTruckLocatorService _foodTruckLocatorService;

        public FoodTruckLocatorController(ILogger<FoodTruckLocatorController> logger, IFoodTruckLocatorService foodTruckLocatorService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _foodTruckLocatorService = foodTruckLocatorService ?? throw new ArgumentNullException(nameof(foodTruckLocatorService));
        }

        [HttpGet]
        public async Task<IEnumerable<MobileFoodFacility>> Get()
        {
            return await _foodTruckLocatorService.ReadAllAsync();
        }

        [HttpGet]
        [Route("nearby")]
        public async Task<IEnumerable<MobileFoodFacility>> GetNearbyTrucks(double latitude, double longitude)
        {
            var result = await _foodTruckLocatorService.LocateNearbyTrucksAsync(latitude, longitude);
            return result;
        }
    }
}
