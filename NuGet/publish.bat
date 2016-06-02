set V=2.10.3
nuget push VirtoCommerce.Platform.Core.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Core.Web.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Data.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Data.Azure.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Data.Notifications.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Data.Security.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Data.Serialization.%V%.nupkg -Source nuget.org -ApiKey %1
nuget push VirtoCommerce.Platform.Testing.%V%.nupkg -Source nuget.org -ApiKey %1
pause
