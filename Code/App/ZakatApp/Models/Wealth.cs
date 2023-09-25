namespace ZakatApp.Models;

public class Wealth
{
    public string standard { get; set; }
    public List<Saving> savings { get; set; }

    public Wealth()
    {
        savings = new List<Saving>();
    }
}

public class Saving
{
    public string Code { get; set; }
    public double Value { get; set; }
}
