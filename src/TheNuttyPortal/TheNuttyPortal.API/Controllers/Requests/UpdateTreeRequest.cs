using TheNuttyPortal.API.Models.Enums;

namespace TheNuttyPortal.API.Controllers.Requests;

public class UpdateTreeRequest
{
    public string TreeName { get; set; } = string.Empty;
    public TreeType TreeType { get; set; }
    public int NutCount { get; set; }
}