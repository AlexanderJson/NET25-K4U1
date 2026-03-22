Set-Location "$PSScriptRoot\.."

Write-Host "Checking SDK.. " -ForegroundColor Cyan
dotnet --version

Write-Host "Updating NuGet packages to latest stable..." -ForegroundColor Cyan


if (Get-Command "dotnet-outdated" -ErrorAction SilentlyContinue) {
    dotnet-outdated -u
} else {
    dotnet list package --outdated
}

Write-Host "Cleaning and restoring deps.." -ForegroundColor Cyan
dotnet clean --nologo
dotnet restore

Write-Host "Validating Framework & Language standards..." -ForegroundColor Cyan
dotnet build /p:Configuration=Debug --nologo

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nREPO is now updated!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" -ForegroundColor Green
} else {
    Write-Host "`nBuild failed. Check violations above." -ForegroundColor Red
    exit $LASTEXITCODE
}