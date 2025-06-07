namespace TheNuttyPortal.API.Controllers.Requests;

public class UpdateTreeRequest
{
    public string TreeName { get; set; } = string.Empty;
    public string TreeType { get; set; } = string.Empty;
    public int NutCount { get; set; }
    public string Ripeness { get; set; } = string.Empty;
}