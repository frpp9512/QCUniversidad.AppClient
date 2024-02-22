#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QCUniversidad.WebClient/QCUniversidad.WebClient.csproj", "QCUniversidad.WebClient/"]
COPY ["QCUniversidad.Api.Shared/QCUniversidad.Api.Shared.csproj", "QCUniversidad.Api.Shared/"]
COPY ["SmartB1t.Security.Extensions/SmartB1t.Security.Extensions.csproj", "SmartB1t.Security.Extensions/"]
COPY ["SmartB1t.Security.WebSecurity.Local/SmartB1t.Security.WebSecurity.Local.csproj", "SmartB1t.Security.WebSecurity.Local/"]
COPY ["SmartB1t.Security/SmartB1t.Security.csproj", "SmartB1t.Security/"]
COPY ["SmartB1t.Web.Extensions/SmartB1t.Web.Extensions.csproj", "SmartB1t.Web.Extensions/"]
RUN dotnet restore "./QCUniversidad.WebClient/QCUniversidad.WebClient.csproj"
COPY . .
WORKDIR "/src/QCUniversidad.WebClient"
RUN dotnet build "./QCUniversidad.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QCUniversidad.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QCUniversidad.WebClient.dll"]