# Reqnroll + Allure setup

This project is configured to publish Allure result files whenever tests run.

## What is configured

- NuGet package: `Allure.Reqnroll`
- Reqnroll runtime plugin config: `reqnroll.json`
- Allure output config: `allureConfig.json` (`allure-results` folder)
- Generated HTML report: `TheNuttyPortal.AppcetanceTests\\allure-report\\index.html`

## Run tests and generate a report

From `src/TheNuttyPortal` run:

```powershell
.\run-reqnroll-tests-with-allure.ps1
```

To open the report automatically after generation:

```powershell
.\run-reqnroll-tests-with-allure.ps1 -OpenReport
```

The script prints both the raw results path and the generated report path on every run.

To avoid stale retry-only data, the script clears old `bin/**/allure-results` folders before each run.

`-OpenReport` now opens the report via `allure open` (local HTTP server), which avoids `file://` CORS errors in modern browsers.

If you open `index.html` directly from disk and see CORS errors, start the local server manually instead:

```powershell
allure open .\TheNuttyPortal.AppcetanceTests\allure-report
```

## Install Allure CLI (Windows)

Pick one:

```powershell
choco install allure-commandline
```

```powershell
scoop install allure
```

Allure CLI also requires Java to generate the HTML report. For example:

```powershell
choco install temurin17
```

After installing, restart your shell so `allure` is on `PATH`.

## Troubleshooting

If the report opens but shows no tests:

- Run the script again so it can regenerate from a fresh `allure-results` folder.
- Open via HTTP (`allure open ...`) instead of opening `index.html` directly.
- Confirm the report stats file has test counts:

```powershell
Get-Content .\TheNuttyPortal.AppcetanceTests\allure-report\widgets\statistic.json
```
