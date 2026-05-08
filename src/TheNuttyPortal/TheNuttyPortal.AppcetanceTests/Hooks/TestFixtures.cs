using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;
using TheNuttyPortal.API;

namespace TheNuttyPortal.AppcetanceTests.Hooks;

[Binding]
public class TestFixtures
{
    [BeforeScenario]
    public static void SetupHttpClient(ScenarioContext scenarioContext)
    {
        var factory = new WebApplicationFactory<Program>();
        var httpClient = factory.CreateClient();
        scenarioContext.ScenarioContainer.RegisterInstanceAs(httpClient);
        scenarioContext.ScenarioContainer.RegisterInstanceAs(factory);
    }

    [AfterScenario]
    public static void TearDownHttpClient(ScenarioContext scenarioContext)
    {
        if (scenarioContext.ScenarioContainer.IsRegistered<WebApplicationFactory<Program>>())
        {
            var factory = scenarioContext.ScenarioContainer.Resolve<WebApplicationFactory<Program>>();
            factory?.Dispose();
        }
    }
}



