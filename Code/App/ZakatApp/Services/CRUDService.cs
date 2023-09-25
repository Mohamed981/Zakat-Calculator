using System.Text;
using System.Text.Json;

namespace ZakatApp.Services;

public abstract class CRUDService<T> where T : class
{
    private readonly HttpClient httpClient = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();
    private readonly string baseURL = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5209/api/" : "http://localhost:5000/api/";

    public CRUDService()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<List<T>> GetItems(string controllerName)
    {

        string url = baseURL + controllerName;

        HttpResponseMessage response = await httpClient.GetAsync($"{url}");
        string content = await response.Content.ReadAsStringAsync();

        Result<List<T>> result = JsonSerializer.Deserialize<Result<List<T>>>(content, _jsonSerializerOptions);

        return result.message;
     
    }
    public async Task<double> PostZakat(Wealth item,string controllerName)
    {
        string url = baseURL + controllerName;
        string jsonItem = JsonSerializer.Serialize<Wealth>(item, _jsonSerializerOptions);
        StringContent content = new StringContent(jsonItem, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync($"{url}", content);
       
        string s = await response.Content.ReadAsStringAsync();
        Result<double> result = JsonSerializer.Deserialize<Result<double>>(s, _jsonSerializerOptions);

        return result.message;
    }
}
