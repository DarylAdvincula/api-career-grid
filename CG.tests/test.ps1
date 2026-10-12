[CmdletBinding()]
param(
    [string]$Filter,
    [switch]$DetailedErrors,
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop"
$resultsDirectory = Join-Path $PSScriptRoot "TestResults"
New-Item -ItemType Directory -Path $resultsDirectory -Force | Out-Null
$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss-fff"
$trxName = "test-results-$timestamp.trx"
$trxPath = Join-Path $resultsDirectory $trxName
$arguments = @(
    "test",
    "CG.Tests.csproj",
    "--logger",
    "trx;LogFileName=$trxName",
    "--results-directory",
    "TestResults"
)

if ($Filter) {
    $arguments += @("--filter", $Filter)
}

if ($NoBuild) {
    $arguments += "--no-build"
}

Push-Location $PSScriptRoot
$previousErrorActionPreference = $ErrorActionPreference
try {
    $ErrorActionPreference = "Continue"
    $dotnetOutput = & dotnet @arguments 2>&1
    $dotnetExitCode = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $previousErrorActionPreference
    Pop-Location
}

if (-not (Test-Path -LiteralPath $trxPath)) {
    Write-Host "Test run did not produce a TRX report (dotnet exit code: $dotnetExitCode)." -ForegroundColor Red
    $dotnetOutput | Select-Object -Last 12 | ForEach-Object { Write-Host $_ }
    exit 1
}

[xml]$trx = Get-Content -LiteralPath $trxPath -Raw
$namespaceUri = $trx.DocumentElement.NamespaceURI
$testResults = @($trx.GetElementsByTagName("UnitTestResult", $namespaceUri))
$passedCount = 0
$failedCount = 0
$skippedCount = 0
$testCases = [System.Collections.Generic.List[object]]::new()
$failureDetails = [System.Collections.Generic.List[object]]::new()
$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path

function Get-AssertionSummary {
    param(
        [string]$Message,
        [string]$TestName
    )

    $exceptionMatch = [regex]::Match($Message, '^Expected a <(?<expected>[^>]+)> to be thrown, but (?<actual>.+?)\.$')
    if ($exceptionMatch.Success) {
        return [pscustomobject]@{
            Expected = "Exception: $($exceptionMatch.Groups['expected'].Value)"
            Actual = $exceptionMatch.Groups['actual'].Value
        }
    }

    $containsMatch = [regex]::Match($Message, '^Expected (?<subject>\w+) "(?<observed>.*)" to contain "(?<expected>.*)"\.$')
    if ($containsMatch.Success) {
        return [pscustomobject]@{
            Expected = "$($containsMatch.Groups['subject'].Value) to contain `"$($containsMatch.Groups['expected'].Value)`""
            Actual = "$($containsMatch.Groups['subject'].Value) was `"$($containsMatch.Groups['observed'].Value)`""
        }
    }

    $comparisonMatch = [regex]::Match($Message, '^Expected (?<subject>.+?) (?<expectation>to .+?), but (?<actual>.+?)\.$')
    if ($comparisonMatch.Success) {
        return [pscustomobject]@{
            Expected = "$($comparisonMatch.Groups['subject'].Value) $($comparisonMatch.Groups['expectation'].Value)"
            Actual = $comparisonMatch.Groups['actual'].Value
        }
    }

    if ($TestName -match '_ReturnsNullForMissingName$') {
        return [pscustomobject]@{
            Expected = "Return null when no row has the requested name."
            Actual = $Message
        }
    }

    if ($TestName -match '_ReturnsNullForUnknownId$') {
        return [pscustomobject]@{
            Expected = "Return null when no row has the requested ID."
            Actual = $Message
        }
    }

    if ($TestName -match '_ReturnsEmptyFor') {
        return [pscustomobject]@{
            Expected = "Return an empty collection when no rows match the filter."
            Actual = $Message
        }
    }

    if ($TestName -match '_ExecutesAgainstSqlServer$') {
        return [pscustomobject]@{
            Expected = "The query should execute successfully against SQL Server."
            Actual = $Message
        }
    }

    return [pscustomobject]@{
        Expected = "The test assertion should pass."
        Actual = $Message
    }
}

function Get-SourceLocations {
    param([string]$StackTrace)

    $locations = [System.Collections.Generic.List[object]]::new()
    $seenLocations = @{}
    $locationMatches = [regex]::Matches($StackTrace, '(?<path>[A-Za-z]:\\[^\r\n]+?\.cs):line (?<line>\d+)')

    foreach ($locationMatch in $locationMatches) {
        $path = $locationMatch.Groups['path'].Value
        $line = $locationMatch.Groups['line'].Value
        if ($path.StartsWith($PSScriptRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
            $relativePath = $path.Substring($PSScriptRoot.Length).TrimStart('\', '/') -replace '\\', '/'
            $linkTarget = "../$relativePath#L$line"
            $linkLabel = "${relativePath}:$line"
        }
        elseif ($path.StartsWith($repositoryRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
            $relativePath = $path.Substring($repositoryRoot.Length).TrimStart('\', '/') -replace '\\', '/'
            $linkTarget = "../../$relativePath#L$line"
            $linkLabel = "${relativePath}:$line"
        }
        else {
            $linkTarget = $path
            $linkLabel = "${path}:$line"
        }

        if (-not $seenLocations.ContainsKey($linkTarget)) {
            $seenLocations[$linkTarget] = $true
            $locations.Add([pscustomobject]@{
                Label = $linkLabel
                Target = $linkTarget
            })
        }
    }

    return $locations.ToArray()
}

foreach ($testResult in $testResults) {
    $testName = $testResult.GetAttribute("testName")
    $outcome = $testResult.GetAttribute("outcome")
    $nameMatch = [regex]::Match($testName, '^CG\.Tests\.(?<Suite>[^.]+)\.(?<Area>[^.]+)\.(?<DisplayName>.+)$')

    if ($nameMatch.Success) {
        $suite = $nameMatch.Groups["Suite"].Value
        $area = $nameMatch.Groups["Area"].Value
        $displayName = $nameMatch.Groups["DisplayName"].Value
    }
    else {
        $suite = "Other"
        $area = "Uncategorized"
        $displayName = $testName
    }

    $suiteOrder = switch ($suite) {
        "Unit" { 0 }
        "Integration" { 1 }
        default { 2 }
    }

    switch ($outcome) {
        "Passed" {
            $passedCount++
        }
        "Failed" {
            $failedCount++
            $errorInfo = $testResult.GetElementsByTagName("ErrorInfo", $namespaceUri) | Select-Object -First 1
            if ($null -ne $errorInfo) {
                $messageNode = $errorInfo.GetElementsByTagName("Message", $namespaceUri) | Select-Object -First 1
                $stackNode = $errorInfo.GetElementsByTagName("StackTrace", $namespaceUri) | Select-Object -First 1
                $message = if ($null -ne $messageNode) { $messageNode.InnerText } else { "No error message was recorded." }
                $stackTrace = if ($null -ne $stackNode) { $stackNode.InnerText } else { "No stack trace was recorded." }
                $assertionSummary = Get-AssertionSummary $message $displayName
                $failure = [pscustomobject]@{
                    Category = "$suite / $area"
                    TestName = $displayName
                    Message = $message
                    StackTrace = $stackTrace
                    Expected = $assertionSummary.Expected
                    Actual = $assertionSummary.Actual
                    Locations = @(Get-SourceLocations $stackTrace)
                }
                $failureDetails.Add($failure)
            }
        }
        default {
            $skippedCount++
        }
    }

    $testCases.Add([pscustomobject]@{
        SuiteOrder = $suiteOrder
        Suite = $suite
        Area = $area
        Category = "$suite / $area"
        DisplayName = $displayName
        Outcome = $outcome
        Failure = $failure
    })
}

$orderedTestCases = @($testCases | Sort-Object SuiteOrder, Area, DisplayName)
foreach ($categoryGroup in ($orderedTestCases | Group-Object Category)) {
    $firstTest = $categoryGroup.Group[0]
    Write-Host ""
    Write-Host "$($firstTest.Suite) / $($firstTest.Area)" -ForegroundColor Cyan

    foreach ($testCase in $categoryGroup.Group) {
        switch ($testCase.Outcome) {
            "Passed" { Write-Host "  PASS  $($testCase.DisplayName)" -ForegroundColor Green }
            "Failed" {
                Write-Host "  FAIL  $($testCase.DisplayName)" -ForegroundColor Red
                if ($null -ne $testCase.Failure) {
                    Write-Host "    Expected: $($testCase.Failure.Expected)" -ForegroundColor DarkGray
                    Write-Host "    Actual:   $($testCase.Failure.Actual)" -ForegroundColor Yellow
                }
            }
            default { Write-Host "  SKIP  $($testCase.DisplayName)" -ForegroundColor Yellow }
        }
    }
}

Write-Host ""
Write-Host ("Total: {0}  Passed: {1}  Failed: {2}  Skipped: {3}" -f $testResults.Count, $passedCount, $failedCount, $skippedCount)
Write-Host "Full TRX report: $trxPath"

if ($failureDetails.Count -gt 0) {
    $markdownPath = Join-Path $resultsDirectory "test-failures-$timestamp.md"
    $markdownLines = [System.Collections.Generic.List[string]]::new()
    $markdownLines.Add("# Test Failure Report")
    $markdownLines.Add("")
    $markdownLines.Add(("Total: {0} | Passed: {1} | Failed: {2} | Skipped: {3}" -f $testResults.Count, $passedCount, $failedCount, $skippedCount))

    $previousCategory = $null
    foreach ($failure in ($failureDetails | Sort-Object Category, TestName)) {
        if ($failure.Category -ne $previousCategory) {
            $markdownLines.Add("")
            $markdownLines.Add("## $($failure.Category)")
            $previousCategory = $failure.Category
        }

        $markdownLines.Add("")
        $markdownLines.Add("### $($failure.TestName)")
        $markdownLines.Add("")
        $markdownLines.Add("- **Expected:** $($failure.Expected)")
        $markdownLines.Add("- **Actual:** $($failure.Actual)")
        if ($failure.Locations.Count -gt 0) {
            $sourceLinks = @($failure.Locations | ForEach-Object { "[``$($_.Label)``]($($_.Target))" }) -join ", "
            $markdownLines.Add("- **Source:** $sourceLinks")
        }
        $markdownLines.Add("")
        $markdownLines.Add("<details>")
        $markdownLines.Add("<summary>Full error and stack trace</summary>")
        $markdownLines.Add("")
        $markdownLines.Add('```text')
        $markdownLines.Add($failure.Message)
        if (-not [string]::IsNullOrWhiteSpace($failure.StackTrace)) {
            $markdownLines.Add("")
            $markdownLines.Add($failure.StackTrace.Trim())
        }
        $markdownLines.Add('```')
        $markdownLines.Add("")
        $markdownLines.Add("</details>")
    }

    Set-Content -LiteralPath $markdownPath -Value ($markdownLines -join "`r`n") -Encoding UTF8
    Write-Host "Failure report: $markdownPath"
}

if ($DetailedErrors -and $failureDetails.Count -gt 0) {
    foreach ($failure in $failureDetails) {
        Write-Host ""
        Write-Host "Failure details: $($failure.TestName)" -ForegroundColor Red
        Write-Host $failure.Message
        Write-Host $failure.StackTrace
    }
}

if ($dotnetExitCode -ne 0) {
    exit $dotnetExitCode
}
