#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
COPY wait-for-it.sh /wait-for-it.sh
RUN apt-get update && apt-get install -y dos2unix && dos2unix /wait-for-it.sh && chmod +x /wait-for-it.sh
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/VirtoCommerce.Platform.Web/VirtoCommerce.Platform.Web.csproj", "src/VirtoCommerce.Platform.Web/"]
COPY ["src/VirtoCommerce.Platform.Security/VirtoCommerce.Platform.Security.csproj", "src/VirtoCommerce.Platform.Security/"]
COPY ["src/VirtoCommerce.Platform.Core/VirtoCommerce.Platform.Core.csproj", "src/VirtoCommerce.Platform.Core/"]
COPY ["src/VirtoCommerce.Platform.Data/VirtoCommerce.Platform.Data.csproj", "src/VirtoCommerce.Platform.Data/"]
COPY ["src/VirtoCommerce.Platform.Caching/VirtoCommerce.Platform.Caching.csproj", "src/VirtoCommerce.Platform.Caching/"]
COPY ["src/VirtoCommerce.Platform.Assets.AzureBlobStorage/VirtoCommerce.Platform.Assets.AzureBlobStorage.csproj", "src/VirtoCommerce.Platform.Assets.AzureBlobStorage/"]
COPY ["src/VirtoCommerce.Platform.Modules/VirtoCommerce.Platform.Modules.csproj", "src/VirtoCommerce.Platform.Modules/"]
COPY ["src/VirtoCommerce.Platform.Hangfire/VirtoCommerce.Platform.Hangfire.csproj", "src/VirtoCommerce.Platform.Hangfire/"]
COPY ["src/VirtoCommerce.Platform.Assets.FileSystem/VirtoCommerce.Platform.Assets.FileSystem.csproj", "src/VirtoCommerce.Platform.Assets.FileSystem/"]
RUN dotnet restore "src/VirtoCommerce.Platform.Web/VirtoCommerce.Platform.Web.csproj"
COPY . .
WORKDIR "/src/src/VirtoCommerce.Platform.Web"
RUN dotnet build "VirtoCommerce.Platform.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VirtoCommerce.Platform.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VirtoCommerce.Platform.Web.dll"]
