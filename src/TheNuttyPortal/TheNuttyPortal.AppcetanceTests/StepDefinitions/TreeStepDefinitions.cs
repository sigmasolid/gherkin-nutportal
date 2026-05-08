using System.Text;
using System.Text.Json;
using Reqnroll;
using TheNuttyPortal.API.Controllers.Requests;
using TheNuttyPortal.API.Models;

namespace TheNuttyPortal.AppcetanceTests.StepDefinitions;

[Binding]
public class TreeStepDefinitions
{
    private readonly HttpClient _httpClient;
    private Tree? _tree;
    private HttpResponseMessage? _lastResponse;

    public TreeStepDefinitions(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [Given("the forest has an {string} tree with the name {string} and {int} {string} nuts")]
    public async Task GivenTheForestHasAnTreeWithTheNameAndNuts(string treeType, string treeName, int numberOfNuts, string ripeness)
    {
        var treeRequest = new UpdateTreeRequest
        {
            TreeName = treeName,
            TreeType = treeType,
            NumberOfNuts = numberOfNuts,
            Ripeness = ripeness
        };
        await UpdateTreeAsync(treeRequest);
    }

    [When("I request information about the tree with the name {string}")]
    public async Task WhenIRequestInformationAboutTheTreeWithTheName(string treeName)
    {
        var response = await _httpClient.GetAsync($"/api/tree/{treeName}");
        _lastResponse = response;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            _tree = JsonSerializer.Deserialize<Tree>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            _tree = null;
        }
    }

    [Then("the response should include the tree ID {string} with {int} {string} nuts")]
    public void ThenTheResponseShouldIncludeTheTreeIdWithNuts(string treeName, int numberOfNuts, string ripeness)
    {
        Assert.NotNull(_tree);
        Assert.Equal(treeName, _tree.Name);
        Assert.Equal(numberOfNuts, _tree.NumberOfNuts);
        Assert.Equal(ripeness, _tree.Ripeness);
    }

    [Given("the forest has the following trees:")]
    public async Task GivenTheForestHasTheFollowingTrees(Reqnroll.Table table)
    {
        var trees = table.CreateSet<UpdateTreeRequest>();
        foreach (var treeRequest in trees)
        {
            await UpdateTreeAsync(treeRequest);
        }
    }

    [When("I query the API for the tree with the most ripe nuts of type {string}")]
    public async Task WhenIQueryTheApiForTheTreeWithTheMostRipeNutsOfType(string treeType)
    {
        var response = await _httpClient.GetAsync($"/api/tree/most-ripe-nuts/{treeType}");
        _lastResponse = response;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            _tree = JsonSerializer.Deserialize<Tree>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            _tree = null;
        }
    }

    [Then("the response should return the tree {string}")]
    public void ThenTheResponseShouldReturnTheTree(string treeName)
    {
        Assert.NotNull(_tree);
        Assert.Equal(treeName, _tree.Name);
    }

    [Then("the tree type should be {string}")]
    public void ThenTheTreeTypeShouldBe(string treeType)
    {
        Assert.NotNull(_tree);
        Assert.Equal(treeType, _tree.TreeType);
    }

    [Then("the nut count should be {int}")]
    public void ThenTheNumberOfNutsShouldBe(int numberOfNuts)
    {
        Assert.NotNull(_tree);
        Assert.Equal(numberOfNuts, _tree.NumberOfNuts);
    }

    private async Task UpdateTreeAsync(UpdateTreeRequest treeRequest)
    {
        var json = JsonSerializer.Serialize(treeRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/tree/update-tree", content);
        _lastResponse = response;
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to update tree: {response.StatusCode} - {errorContent}");
        }
    }
}