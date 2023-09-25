
namespace ZakatApp.Services
{
    public interface ICRUDService<T> where T : class
    {
        Task<List<T>> GetItems(string controllerName);
        Task<double> PostZakat(Wealth item,string controllerName);
    }
}
