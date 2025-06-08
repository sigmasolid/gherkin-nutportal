using Microsoft.AspNetCore.Mvc;
using TheNuttyPortal.API.Controllers.Requests;
using TheNuttyPortal.API.Models;

namespace TheNuttyPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreeController : ControllerBase
{
    // Simulate in-memory storage (replace with a database in real app)
    private static readonly Dictionary<string, Tree> _forest = new();

    [HttpPost("update-tree")]
    public ActionResult<Tree> UpdateTree([FromBody] UpdateTreeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TreeName) || request.NutCount < 0)
        {
            return BadRequest("Invalid request");
        }

        var key = ProcessUpdateTreeRequest(request);

        return Ok(_forest[key]);
    }

    private static string ProcessUpdateTreeRequest(UpdateTreeRequest request)
    {
        var key = request.TreeName.ToLowerInvariant();

        if (_forest.ContainsKey(key))
        {
            UpdateNutCount(request, key);
        }
        else
        {
            CreateNewTree(request, key);
        }

        return key;
    }

    private static void CreateNewTree(UpdateTreeRequest request, string key)
    {
        _forest[key] = new Tree
        {
            Name = request.TreeName,
            TreeType = request.TreeType,
            NutCount = request.NutCount,
            Ripeness = request.Ripeness
        };
    }

    private static void UpdateNutCount(UpdateTreeRequest request, string key)
    {
        _forest[key].NutCount = request.NutCount;
    }

    [HttpGet("{treeName}")]
    public ActionResult<Tree> GetTree(string treeName)
    {
        var key = treeName.ToLowerInvariant();

        return _forest.TryGetValue(key, out var tree)
            ? Ok(tree)
            : NotFound($"Tree '{treeName}' not found.");
    }
}