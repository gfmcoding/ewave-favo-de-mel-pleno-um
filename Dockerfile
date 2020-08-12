FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /new-system
COPY ./src .
RUN dotnet build --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /production
COPY --from=build /new-system/WebApi/src/bin/Release/netcoreapp3.1 .
//TODO: CMD != Entrypoint
CMD ["dotnet", "FavorDeMel.WebApi.dll"]
