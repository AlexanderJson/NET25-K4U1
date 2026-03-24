Write-Host "Updating.." -ForegroundColor Cyan


Get-ChildItem -Filter *.csproj -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName
    $content -replace '<TargetFramework>net8.0</TargetFramework>', '<TargetFramework>$(RequiredTargetFramework)</TargetFramework>' | Set-Content $_.FullName
}

Write-Host "Done!" -ForegroundColor Green
