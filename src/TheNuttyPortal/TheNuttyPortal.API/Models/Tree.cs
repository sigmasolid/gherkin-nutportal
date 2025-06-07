namespace TheNuttyPortal.API.Models;

public class Tree
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int NutCount { get; set; }
    public string Ripeness { get; set; } = string.Empty;
}
