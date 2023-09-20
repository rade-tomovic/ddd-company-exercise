#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CompanyManager.Api/CompanyManager.Api.csproj", "src/CompanyManager.Api/"]
COPY ["src/CompanyManager.Application/CompanyManager.Application.csproj", "src/CompanyManager.Application/"]
COPY ["src/CompanyManager.Domain/CompanyManager.Domain.csproj", "src/CompanyManager.Domain/"]
RUN dotnet restore "src/CompanyManager.Api/CompanyManager.Api.csproj"
COPY . .
WORKDIR "/src/src/CompanyManager.Api"
RUN dotnet build "CompanyManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyManager.Api.dll"]