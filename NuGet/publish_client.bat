set V=2.10.5
nuget push VirtoCommerce.Platform.Client.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Client.Security.%V%.nupkg -Source nuget.org -ApiKey %1
pause
