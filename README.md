# The food truck locator

This is my little attempt at solving the take home engineering challenge. By no mean this is a final solution or what I've implemented would be according to best practices, however, I have tried to find a balance between a good and a practical solution.

## Solution structure

I decided to add three projects, one for the web, one for the API and another for sharing the models.

### The API and Model project setup

Beginning with the API I started with the **ASP.Net Core API** template, then thought about parsing the `CSV` file with the `TinyCSVParser` which is well known to handle the task very well. For Geo Location I searched a bit and found an implementation for DotNet Core which I reviewed and decided to add to the project.

Next, I added the a class to represent each row in the CSV file called `MobileFoodFacility` in the **Model** project and added a project reference to the **API** project. This will allow me to reuse this later in the **Web** project where I have to get it from the API and iterate through the result.

Then I added an interface called `ICSVParserService` for the CSV parser service which has only one method called `ReadAll`. Next, a class inheriting that interface was added which is implementing the method using the `TinyCSVParser` library to get a file and return a list of `MobileFoodFacility`.

To avoid reading the file again and again for each API call, I added the in-memory cache as well to store the list in there after the first read. The plan is to check the cache first before calling this class.

Next I added an interface for the main service called `IFoodTruckLocatorService` which has two method, one is called `ReadAllAsync` which returns everything and one to get nearby trucks called `LocateNearbyTrucksAsync`.

In the `FoodTruckLocatorService` itself I have injected the in-memory cache and the CSV parser service and in its method, we first check the cache and if there is no data in there we will call the CSV parser service.

The only difference between these two methods is that `LocateNearbyTrucksAsync` will get a latitude and a longitude input to use in it's query. Here I had to use the `GeoCoordinatePortable` class to be able to order by the closest trucks and get the top five.

I could have passed the top `x` value from the caller, but that would be for a future version.

The next thing to do was to create an application registration representing the API and add the client id and tenant id in the `appsettings.json` file.

This app registration will expose an API with `Trucks.Read` scope which will be used by the web when calling the API.

I added another app registration would be for the web which has the permissions to call our API and has the `localhost` URL as its return URI.

This app also needs a secret for the Web project to be able to get a token and send it along with the API requests.

After adding the app registration, I added the `ClientId`, `TenantId`, and the `Domain` to the `appsettings.json` file.

Next was time to modify the `startup.cs` file to setup authentication, OpenAPI docs using the `Swashbuckle` library, and also register our services for the dependency injector to be available.

I tested the API manually and it was returning the results as intended.

TODO:\\\

- Add tests for the API
