[CmdletBinding()]
param(
    [ValidateRange(1, 3600)]
    [int] $TimeoutSeconds = 180,

    [ValidateRange(0, 5)]
    [int] $LogLevel = 5
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$orangeDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $orangeDirectory
$orangePython = Join-Path $env:LOCALAPPDATA 'Programs\Orange\python.exe'

if (-not (Test-Path -LiteralPath $orangePython -PathType Leaf)) {
    $orangePython = Get-ChildItem `
        -LiteralPath (Join-Path $env:LOCALAPPDATA 'Programs\Orange') `
        -Filter 'python.exe' `
        -File `
        -Recurse `
        -ErrorAction SilentlyContinue |
        Select-Object -First 1 -ExpandProperty FullName
}

if (-not $orangePython) {
    throw 'Orange standalone python.exe was not found.'
}

$logDirectory = Join-Path $projectRoot 'outputs\orange-cli-check'
New-Item -ItemType Directory -Path $logDirectory -Force | Out-Null
$utf8NoBom = [System.Text.UTF8Encoding]::new($false)

function Invoke-OrangeWorkflowCheck {
    param(
        [Parameter(Mandatory)]
        [string] $RelativeWorkflow
    )

    $workflowPath = [System.IO.Path]::GetFullPath(
        (Join-Path $projectRoot $RelativeWorkflow)
    )
    $rootPrefix = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd('\') + '\'
    if (-not $workflowPath.StartsWith(
        $rootPrefix,
        [System.StringComparison]::OrdinalIgnoreCase
    )) {
        throw "Workflow is outside the project root: $workflowPath"
    }
    if (-not (Test-Path -LiteralPath $workflowPath -PathType Leaf)) {
        throw "Workflow was not found: $workflowPath"
    }

    $workflowName = [System.IO.Path]::GetFileNameWithoutExtension($workflowPath)
    $stdoutPath = Join-Path $logDirectory "$workflowName.stdout.log"
    $stderrPath = Join-Path $logDirectory "$workflowName.stderr.log"

    # Pass only an ASCII relative path through QApplication.arguments().
    # Qt 5 corrupts non-ASCII characters in the absolute workspace path.
    $relativeArgument = $RelativeWorkflow.Replace('/', '\')
    $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = $orangePython
    $startInfo.WorkingDirectory = $projectRoot
    $startInfo.Arguments = "-m Orange.canvas.run -l $LogLevel `"$relativeArgument`""
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.EnvironmentVariables['PYTHONIOENCODING'] = 'utf-8'

    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo
    Write-Host "Checking: $RelativeWorkflow"

    try {
        if (-not $process.Start()) {
            throw "Could not start Orange for $RelativeWorkflow"
        }
        $stdoutTask = $process.StandardOutput.ReadToEndAsync()
        $stderrTask = $process.StandardError.ReadToEndAsync()
        $completed = $process.WaitForExit($TimeoutSeconds * 1000)

        if (-not $completed) {
            try {
                $process.Kill()
            }
            catch {
                Write-Warning "Could not stop process $($process.Id): $_"
            }
            $process.WaitForExit()
            $exitCode = 124
        }
        else {
            $process.WaitForExit()
            $exitCode = [int] $process.ExitCode
        }

        $stdout = $stdoutTask.GetAwaiter().GetResult()
        $stderr = $stderrTask.GetAwaiter().GetResult()
        [System.IO.File]::WriteAllText($stdoutPath, $stdout, $utf8NoBom)
        [System.IO.File]::WriteAllText($stderrPath, $stderr, $utf8NoBom)
    }
    finally {
        $process.Dispose()
    }

    if ($exitCode -eq 0) {
        Write-Host "PASS: $workflowName (exit 0)"
    }
    elseif ($exitCode -eq 124) {
        Write-Host "TIMEOUT: $workflowName after $TimeoutSeconds seconds"
    }
    else {
        Write-Host "FAIL: $workflowName (exit $exitCode)"
    }
    Write-Host "  stdout: $stdoutPath"
    Write-Host "  stderr: $stderrPath"
    return $exitCode
}

Write-Host "Orange Python: $orangePython"
$workflows = @(
    'orange\classification-tree.ows',
    'orange\regression-tree.ows',
    'orange\wine_poly_red.ows',
    'orange\wine_poly_white.ows'
)
$results = [ordered]@{}
foreach ($workflow in $workflows) {
    $results[$workflow] = Invoke-OrangeWorkflowCheck `
        -RelativeWorkflow $workflow
}

Write-Host 'Summary:'
foreach ($entry in $results.GetEnumerator()) {
    Write-Host "  $($entry.Key): $($entry.Value)"
}

if ($results.Values -contains 124) {
    Write-Error 'At least one Orange workflow timed out. See outputs\orange-cli-check.'
    exit 124
}
if ($results.Values | Where-Object { $_ -ne 0 }) {
    Write-Error 'At least one Orange CLI check failed. See outputs\orange-cli-check.'
    exit 1
}

Write-Host 'PASS: All four workflows passed Orange.canvas.run.'
