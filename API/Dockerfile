#build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -o /app

#final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
#from=build is connection from build to final
COPY --from=build /app .
ENTRYPOINT ["dotnet", "API.dll"]
