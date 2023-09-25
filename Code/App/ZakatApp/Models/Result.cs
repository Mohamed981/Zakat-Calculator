
namespace ZakatApp.Models;

public class Result<T>
{
    public string status { get; set; }
    public T message { get; set; }
}
