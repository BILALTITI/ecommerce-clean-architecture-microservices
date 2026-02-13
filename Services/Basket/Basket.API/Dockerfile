# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["E-Commerace/Services/Basket/Basket.API/Basket.API.csproj", "E-Commerace/Services/Basket/Basket.API/"]
COPY ["E-Commerace/Services/Basket/Basket.Application/Basket.Application.csproj", "E-Commerace/Services/Basket/Basket.Application/"]
COPY ["E-Commerace/Services/Basket/Basket.Core/Basket.Core.csproj", "E-Commerace/Services/Basket/Basket.Core/"]
COPY ["E-Commerace/Services/Basket/Basket.Infrasture/Basket.Infrasture.csproj", "E-Commerace/Services/Basket/Basket.Infrasture/"]
COPY ["EventBusMessages/EventBusMessages.csproj", "EventBusMessages/"]	
COPY ["Common.Logging/Common.Logging.csproj", "Common.Logging/"]
RUN dotnet restore "./E-Commerace/Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/E-Commerace/Services/Basket/Basket.API"
RUN dotnet build "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]