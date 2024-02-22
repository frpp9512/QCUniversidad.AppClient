#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QCUniversidad.Api/QCUniversidad.Api.csproj", "QCUniversidad.Api/"]
COPY ["QCUniversidad.Api.Shared/QCUniversidad.Api.Shared.csproj", "QCUniversidad.Api.Shared/"]
RUN dotnet restore "./QCUniversidad.Api/QCUniversidad.Api.csproj"
COPY . .
WORKDIR "/src/QCUniversidad.Api"
RUN dotnet build "./QCUniversidad.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QCUniversidad.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QCUniversidad.Api.dll"]