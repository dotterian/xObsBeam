using System.Text.Json;
using System.Net.Http;

namespace xObsBeam;

public class IPHandler
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    static readonly HttpClient client = new HttpClient();

    public async Task<string> ReadIpFromStorage(string host, string nickname)
    {
    // Call asynchronous network methods in a try/catch block to handle exceptions.
    // https://rwz6gk0sea.execute-api.us-east-1.amazonaws.com/dev/xobsbeam/
    try
    {
        var response = await client.GetAsync("https://" + host + "/dev/xobsbeam/" + nickname);
        string responseBody = await response.Content.ReadAsStringAsync();
        Module.Log($"{responseBody}", ObsLogLevel.Debug);
        return responseBody;
    }
    catch (HttpRequestException e)
    {
        Module.Log($"\nException Caught! Message :{e.Message}", ObsLogLevel.Error);
        return null;
    }
    }

    public async Task WriteToStorage(string host, string nickname, string ipAddress)
    {
    // Call asynchronous network methods in a try/catch block to handle exceptions.
    // https://rwz6gk0sea.execute-api.us-east-1.amazonaws.com/dev/xobsbeam/
    try
    {        
        using var response = await client.PutAsync("https://" + host + "/dev/xobsbeam/" + nickname, new StringContent(ipAddress));
        string ipReturned = await response.Content.ReadAsStringAsync();
        Module.Log($"{ipReturned}", ObsLogLevel.Debug);
    }
    catch (HttpRequestException e)
    {
        Module.Log($"\nException Caught! Message :{e.Message}", ObsLogLevel.Error);
    }
    }
}