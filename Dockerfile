FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /favo-de-mel
COPY ./src .
RUN dotnet build --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /production
COPY --from=build /favo-de-mel/WebApi/bin/Release/netcoreapp3.1 .
CMD ["dotnet", "FavoDeMel.WebApi.dll"]
