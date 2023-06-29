
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0
// on arch needs the package: aspnet-runtime 

// https://swimburger.net/blog/dotnet/how-to-run-a-dotnet-core-console-app-as-a-service-using-systemd-on-linux


using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
string bearerToken = Environment.GetEnvironmentVariable("HASS_BEARER_TOKEN");
string hassAddress = Environment.GetEnvironmentVariable("HASS_ADDRESS");
string streamURI = Environment.GetEnvironmentVariable("STREAM_URI");
const string scriptName = "script.lounge2_set_channel";

app.MapGet("/channel/{name}", (string name) =>
{
    using (HttpClient client = new HttpClient())
    {
        string json = "{\"entity_id\": \"" + scriptName + "\", \"variables\": {\"channel\":\"" + name + "\"}}";
        string apiUrl = $"{hassAddress}/api/services/script/turn_on";
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        var hassresponse = client.PostAsync(apiUrl, body).GetAwaiter().GetResult();
    }
    return Results.Redirect($"http://192.168.1.217/live/stream0", false, false);
});

app.Run("http://localhost:3212");