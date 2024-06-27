#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WareHouseManagement.API/WareHouseManagement.API.csproj", "WareHouseManagement.API/"]
COPY ["WareHouseManagement.Repository/WareHouseManagement.Repository.csproj", "WareHouseManagement.Repository/"]
RUN dotnet restore "./WareHouseManagement.API/WareHouseManagement.API.csproj"
COPY . .
WORKDIR "/src/WareHouseManagement.API"
RUN dotnet build "./WareHouseManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WareHouseManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WareHouseManagement.API.dll"]