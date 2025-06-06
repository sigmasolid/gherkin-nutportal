using TheNuttyPortal.API.Models.Enums;

namespace TheNuttyPortal.API.Models;

public class Tree
{
    public string Name { get; set; } = string.Empty;
    public TreeType Type { get; set; }
    public int NutCount { get; set; }
}
