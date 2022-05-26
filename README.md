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

## The Web project setup

For the Web project I decided to use `Blazor` as I hadn't had a chance to touch it for a long time (nearly 4 years) which resulted in me finding myself down a rabbit hole half way through since I needed to get access to browser APIs and it wasn't easy to find information on how to do so.

First thing first I added the app registration information in the `appsettings.json` file. I also had to add the API app registration info separately including the API URI, required scope and the resource id.

Since I needed to get a hold of the access_token, I had to to create a token provider and setup the `Blazor` app to get it using the context, however, that didn't work until I found out that I had to add a configuration in the `startup.cs` file to tell the app it should save the token for later use and also specify the API resource and its scope to be included in the token as well.

Then I had to add the necessary code to the `_Host.chtml` file to not only get the token from `HttpContext`, but also the JavaScript method to use the `geolocation API` and get the current coordinates of the user expose it as a method so I can call it from my `Blazor` app and get the coordinates (StackOverflow saved my bacon here 😎).

Next I created a service called `HttpClientService` to be able to make HTTP calls to the API. Within that service I needed to inject the token provider to be able to get the access token. This service also had a method which gets the latitude and longitude, then construct a request and sends it to the API to get the nearby trucks.

Then I cleaned up the unnecessary default pages and started the UI in the `Index.razor` file. First I added three inputs to hold the latitude, longitude, and accuracy (these are passed from JavaScript).

Next was to inject the `IJSRuntime` to be able to call the JavaScript function I defined earlier. Here I had to work around the fact that serialization is not the same between the browser APIs and `C#`.

I used the `OnAfterRenderAsync` method to get the coordinates from `JavaScript` and then update the UI.

Next I had to put a button there to call the API and show the results. After a few back and forth things started to work and I was happy with the results. This was after roughly half a day BTW.

I added a flag later to enable to inputs so that if someone wants to enter the latitude and longitude manually then would be able to do so.

Again, I didn't get a chance to write tests for it as well.

## Things that I would do differently

- I would definitely use local secret for the app registration secret and a `keyvault` ref if the app is deployed to **Azure**.

- Writing tests would be a definite especially integration tests between the Web and the API projects, unit tests for the API and UI tests for the Blazor app.

- I would also clean up the code a bit and make it more reusable.
