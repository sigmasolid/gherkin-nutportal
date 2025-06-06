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
    public IActionResult UpdateTree([FromBody] UpdateTreeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TreeName) || request.NutCount < 0)
        {
            return BadRequest("Invalid request");
        }

        var key = request.TreeName.ToLowerInvariant();

        if (_forest.ContainsKey(key))
        {
            _forest[key].NutCount = request.NutCount;
        }
        else
        {
            _forest[key] = new Tree
            {
                Name = request.TreeName,
                Type = request.TreeType,
                NutCount = request.NutCount
            };
        }

        return Ok(_forest[key]);
    }

    [HttpGet("{treeName}")]
    public IActionResult GetTree(string treeName)
    {
        var key = treeName.ToLowerInvariant();

        return _forest.TryGetValue(key, out var tree)
            ? Ok(tree)
            : NotFound($"Tree '{treeName}' not found.");
    }
}