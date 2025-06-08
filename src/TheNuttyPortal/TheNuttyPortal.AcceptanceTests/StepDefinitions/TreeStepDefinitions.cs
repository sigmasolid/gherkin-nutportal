using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Reqnroll;
using TheNuttyPortal.API.Controllers;
using TheNuttyPortal.API.Controllers.Requests;
using TheNuttyPortal.API.Models;

namespace TheNuttyPortal.AcceptanceTests.StepDefinitions;

[Binding]
public class TreeStepDefinitions()
{
    private readonly HttpClient httpClient = new (){BaseAddress = new Uri("http://localhost:5135")};
    private TreeController _treeController = new ();
    private Tree _tree;
    private int? _responseHttpCode;
    
    [Given("the forest has the following trees:")]
    public void GivenTheForestHasTheFollowingTrees(Table table)
    {
        //Convert table to a list of trees
        var trees = table.CreateSet<UpdateTreeRequest>().ToList();
        trees.ForEach(tree => httpClient.PostAsJsonAsync("api/tree/update-tree", tree));
    }

    [Given("the forest has an {string} tree with the name {string} and {int} {string} nuts")]
    public void GivenTheForestHasAnTreeWithTheNameAndNuts(string treeType, string treeName, int nutCount, string ripeness)
    {
        // Create a tree with the specified properties
        var request = new UpdateTreeRequest
        {
            TreeType = treeType,
            TreeName = treeName,
            NutCount = nutCount,
            Ripeness = ripeness
        };
        var treeController = new TreeController();
        treeController.UpdateTree(request);
    }

    [When("I request information about the tree with the name {string}")]
    public void WhenIRequestInformationAboutTheTreeWithTheName(string treeName)
    {
        var result = _treeController.GetTree(treeName);
        if (result.Result is OkObjectResult okObjectResult)
        {
            _tree = okObjectResult.Value as Tree;
        }
    }

    [Then("the response should include the tree ID {string} with {int} {string} nuts")]
    public void ThenTheResponseShouldIncludeTheTreeIdWithNuts(string treeName, int expectedNutCount, string expectedRipeness)
    {
        // Check if the tree exists and has the expected properties
        Assert.NotNull(_tree);
        Assert.Equal(treeName, _tree.Name);
        Assert.Equal(expectedNutCount, _tree.NutCount);
        Assert.Equal(expectedRipeness, _tree.Ripeness);
    }

    [Given("the forest has no trees")]
    public void GivenTheForestHasNoTrees()
    {
        
    }

    [When("I request information about a tree with the name {string}")]
    public void WhenIRequestInformationAboutATreeWithTheName(string treeName)
    {
        var result = _treeController.GetTree(treeName);
        var notFoundResult = result.Result as NotFoundObjectResult;
        _responseHttpCode = notFoundResult?.StatusCode;
    }

    [Then("the response should indicate that the tree does not exist")]
    public void ThenTheResponseShouldIndicateThatTheTreeDoesNotExist()
    {
        Assert.Equal(404, _responseHttpCode);
    }
}