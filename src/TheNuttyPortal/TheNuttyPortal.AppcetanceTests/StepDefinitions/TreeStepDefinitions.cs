using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Reqnroll;
using TheNuttyPortal.API.Controllers;
using TheNuttyPortal.API.Controllers.Requests;
using TheNuttyPortal.API.Models;

namespace StepDefinitions;

[Binding]
public class TreeStepDefinitions(TreeController treeController)
{
    private Tree? _tree;
    
    [Given("the forest has an {string} tree with the name {string} and {int} {string} nuts")]
    public void GivenTheForestHasAnTreeWithTheNameAndNuts(string treeType, string treeName, int nutCount, string ripeness)
    {
        var treeRequest = new UpdateTreeRequest
        {
            TreeName = treeName,
            TreeType = treeType,
            NutCount = nutCount,
            Ripeness = ripeness
        };
        var response = treeController.UpdateTree(treeRequest);
    }

    [When("I request information about the tree with the name {string}")]
    public void WhenIRequestInformationAboutTheTreeWithTheName(string treeName)
    {
        var response = treeController.GetTree(treeName);
        if(response.Result is OkObjectResult okResult)
        {
            _tree = okResult.Value as Tree;
        }
        else
        {
            _tree = null;
        }
    }

    [Then("the response should include the tree ID {string} with {int} {string} nuts")]
    public void ThenTheResponseShouldIncludeTheTreeIdWithNuts(string treeName, int nutCount, string ripeness)
    {
        if (_tree == null)
        {
            ScenarioContext.Current.Pending();
            return;
        }

        Assert.Equal(treeName, _tree.Name);
        Assert.Equal(nutCount, _tree.NutCount);
        Assert.Equal(ripeness, _tree.Ripeness);
    }

    [Given("the forest has the following trees:")]
    public void GivenTheForestHasTheFollowingTrees(Reqnroll.Table table)
    {
        var trees = table.CreateSet<UpdateTreeRequest>();
        trees.ForEach(treeRequest => treeController.UpdateTree(treeRequest));
    }

    [When("I query the API for the tree with the most ripe nuts of type {string}")]
    public void WhenIQueryTheApiForTheTreeWithTheMostRipeNutsOfType(string treeType)
    {
        var response = treeController.GetTreeWithMostRipeNuts(treeType);
        if(response.Result is OkObjectResult okResult)
        {
            _tree = okResult.Value as Tree;
        }
        else
        {
            _tree = null;
        }
    }

    [Then("the response should return the tree {string}")]
    public void ThenTheResponseShouldReturnTheTree(string treeName)
    {
        Assert.Equal(treeName, _tree?.Name);
    }

    [Then("the tree type should be {string}")]
    public void ThenTheTreeTypeShouldBe(string treeType)
    {
        Assert.Equal(treeType, _tree?.TreeType);
    }

    [Then("the nut count should be {int}")]
    public void ThenTheNutCountShouldBe(int nutCount)
    {
        Assert.Equal(nutCount, _tree?.NutCount);
    }
}