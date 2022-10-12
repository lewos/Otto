#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otto.companies/Otto.companies.csproj", "Otto.companies/"]
COPY ["Otto.models/Otto.models.csproj", "Otto.models/"]
RUN dotnet restore "Otto.companies/Otto.companies.csproj"
COPY . .
WORKDIR "/src/Otto.companies"
RUN dotnet build "Otto.companies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otto.companies.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Otto.companies.dll"]