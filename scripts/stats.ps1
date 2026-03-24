param(
    [string]$ProjectName,
    [string]$Configuration,
    [string]$Framework,
    [string]$Status
)

$root = Split-Path -Parent $PSScriptRoot
$logDir = Join-Path $root "logs"
$logFile = Join-Path $logDir "$ProjectName.log"

if (!(Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir | Out-Null
}

try {
    $files = Get-ChildItem -Recurse -Include *.cs
    $fileCount = $files.Count
    $lineCount = ($files | Get-Content | Measure-Object -Line).Lines
} catch {
    $fileCount = "N/A"
    $lineCount = "N/A"
}

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

$logLine = "[$timestamp] STATUS=$Status | Project=$ProjectName | Config=$Configuration | Framework=$Framework | Files=$fileCount | Lines=$lineCount"

Add-Content -Path $logFile -Value $logLine

Write-Output $logLine