param(
    [switch]$OpenReport
)

$ErrorActionPreference = "Stop"

$acceptanceProjectRoot = Join-Path $PSScriptRoot "TheNuttyPortal.AppcetanceTests"
$projectPath = Join-Path $PSScriptRoot "TheNuttyPortal.AppcetanceTests\TheNuttyPortal.AppcetanceTests.csproj"
$resultsSearchRoot = Join-Path $PSScriptRoot "TheNuttyPortal.AppcetanceTests\bin"
$reportDir = Join-Path $PSScriptRoot "TheNuttyPortal.AppcetanceTests\allure-report"
$reportIndexPath = Join-Path $reportDir "index.html"

function Open-GeneratedReport {
    param(
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.CommandInfo]$AllureCommand,
        [Parameter(Mandatory = $true)]
        [string]$OpenWorkingDirectory
    )

    Write-Host "Opening Allure report over HTTP from: $OpenWorkingDirectory"

    $allurePath = $AllureCommand.Source
    $allureExtension = [System.IO.Path]::GetExtension($allurePath)

    if ($allureExtension -ieq ".ps1") {
        $cmdShimPath = [System.IO.Path]::ChangeExtension($allurePath, ".cmd")

        if (Test-Path $cmdShimPath) {
            Start-Process -FilePath $cmdShimPath -ArgumentList @("open") -WorkingDirectory $OpenWorkingDirectory | Out-Null
        }
        else {
            # Execute script-based shims via PowerShell; launching .ps1 directly can open in an editor.
            Start-Process -FilePath "powershell.exe" -ArgumentList @("-NoProfile", "-ExecutionPolicy", "Bypass", "-File", "$allurePath", "open") -WorkingDirectory $OpenWorkingDirectory | Out-Null
        }
    }
    else {
        Start-Process -FilePath $allurePath -ArgumentList @("open") -WorkingDirectory $OpenWorkingDirectory | Out-Null
    }

    return $true
}

Write-Host "Preparing Allure output directories..."
Get-ChildItem -Path $resultsSearchRoot -Directory -Filter "allure-results" -Recurse -ErrorAction SilentlyContinue |
    ForEach-Object {
        Remove-Item -Path $_.FullName -Recurse -Force
    }

Write-Host "Running Reqnroll tests..."
dotnet test "$projectPath"
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$resultsDir = Get-ChildItem -Path $resultsSearchRoot -Directory -Filter "allure-results" -Recurse |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if (-not $resultsDir) {
    Write-Error "No allure-results directory found after running tests."
}

Write-Host "Allure results directory: $($resultsDir.FullName)"
Write-Host "Allure report directory: $reportDir"
Write-Host "Allure report file: $reportIndexPath"

$allure = Get-Command allure -ErrorAction SilentlyContinue
if (-not $allure) {
    Write-Warning "Allure CLI is not installed or not on PATH. Test results were still produced in allure-results."
    Write-Host "Install Allure CLI and Java, then run this script again to generate HTML reports automatically."

    if ($OpenReport) {
        Write-Warning "Cannot auto-open report without Allure CLI. Opening index.html directly may trigger browser CORS errors."
        if (Test-Path $reportIndexPath) {
            Write-Host "If needed, open this file directly: $reportIndexPath"
        }
    }

    exit 0
}

Write-Host "Generating Allure report..."
if (Test-Path $reportDir) {
    Remove-Item -Path $reportDir -Recurse -Force
}
& $allure.Source generate "$($resultsDir.FullName)" -o "$reportDir"
if ($LASTEXITCODE -ne 0) {
    Write-Warning "Allure report generation failed. Verify that Java is installed and available on PATH."
    exit $LASTEXITCODE
}

Write-Host "Allure report generated at: $reportDir"
Write-Host "For CORS-safe viewing from test project root, run: allure open"
Write-Host "Direct file path (may hit CORS in some browsers): $reportIndexPath"

if ($OpenReport) {
    Open-GeneratedReport -AllureCommand $allure -OpenWorkingDirectory $acceptanceProjectRoot | Out-Null
}
