# Brass Loon Rest Client

Brass Loon Rest Client is a JSON REST interface utility built around System.Net.Http.HttpClient.
Its primary function is serializing and de-serializing web API data to and from object data
models.  Version 1.0 supports JWT bearer token authentication.

The component targets .Net Standard and .net 6.

## Usage

The component is distributed via [nuget](https://www.nuget.org/packages/BrassLoon.RestClient).

## IService Interface

This interface defines componenet's core functionality.

Use the CreateRequest method to get an instance of IRequest. IRequest defines
the constrants of an individual web request.

Use the Send method to initiate a web request. The send method returns an IResponse or IResponse\<T>.

Implemented by the Service class.

Here's a simple example:
```
string baseAddress = "http://api.example.com";
string code = "lCode";
object data = new { Code = code, Value = "Lookup Value" };
Func<Task<string>> getToken => Task.FromResult(" bear token ");
IRequest request = _service.CreateRequest(new Uri(baseAddress), HttpMethod.Put, data)
  .AddPath("Lookup/{code}")
  .AddPathParameter("code", codeValue)
  .AddJwtAuthorizationToken(getToken);
IResponse<T> response = await service.Send<T>(request);
// check response status
return response.Value;
```

## License

GNU General Public License 3