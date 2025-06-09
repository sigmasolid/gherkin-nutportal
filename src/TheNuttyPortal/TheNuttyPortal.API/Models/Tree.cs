namespace TheNuttyPortal.API.Models;

public class Tree
{
    public string Name { get; set; } = string.Empty;
    public string TreeType { get; set; } = string.Empty;
    public int NumberOfNuts { get; set; }
    public string Ripeness { get; set; } = string.Empty;
}
