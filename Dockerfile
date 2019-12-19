FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app/

RUN curl -sL https://deb.nodesource.com/setup_11.x | bash - && apt-get install -yq nodejs

COPY . .

WORKDIR /app/src/VirtoCommerce.Platform.Web
RUN npm ci && npm run webpack:build
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "VirtoCommerce.Platform.Web.dll"]
