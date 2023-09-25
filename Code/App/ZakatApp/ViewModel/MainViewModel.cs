
namespace ZakatApp.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly ICurrencyService currencyService;

    public MainViewModel(ICurrencyService currencyService)
    {
        Currencies = new();
        Wealth = new()
        {
            standard = "XAU"
        };
        Savings = new();
        this.currencyService = currencyService;
    }

    [ObservableProperty]
    ObservableCollection<Currency> currencies;

    [ObservableProperty]
    Currency selectedCurrency;

    [ObservableProperty]
    string price;

    [ObservableProperty]
    string standard;

    [ObservableProperty]
    Wealth wealth;

    [ObservableProperty]
    ObservableCollection<Saving> savings;

    [ObservableProperty]
    double result;

    [RelayCommand]
    async Task GetCurrencies()
    {
        List<Currency> currencies = await currencyService.GetItems("currencies");

        Currencies = new ObservableCollection<Currency>(currencies);
    }
    
    [RelayCommand]
    async Task AddSaving()
    {
        Saving s=new Saving
        {
            Code=SelectedCurrency.Code,Value=Convert.ToDouble(Price)
        };
        Savings.Add(s);
        SelectedCurrency = null;
        Price = null;
    }

    [RelayCommand]
    async Task PostZakat()
    {
        Wealth.standard = Standard;
        Wealth.savings = Savings.ToList();
        Result = await currencyService.PostZakat( Wealth, "zakat");
    }
}
