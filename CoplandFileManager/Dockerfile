#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoplandFileManager/CoplandFileManager.csproj", "CoplandFileManager/"]
COPY ["CoplandFileManager.Application/CoplandFileManager.Application.csproj", "CoplandFileManager.Application/"]
COPY ["CoplandFileManager.Domain/CoplandFileManager.Domain.csproj", "CoplandFileManager.Domain/"]
COPY ["CoplandFileManager.Infrastructure/CoplandFileManager.Infrastructure.csproj", "CoplandFileManager.Infrastructure/"]
COPY ["CoplandFileManager.Workers/CoplandFileManager.Workers.csproj", "CoplandFileManager.Workers/"]
RUN dotnet restore "./CoplandFileManager/./CoplandFileManager.csproj"
COPY . .
WORKDIR "/src/CoplandFileManager"
RUN dotnet build "./CoplandFileManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoplandFileManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoplandFileManager.dll"]