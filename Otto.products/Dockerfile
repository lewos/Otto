#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otto.products/Otto.products.csproj", "Otto.products/"]
COPY ["Otto.models/Otto.models.csproj", "Otto.models/"]
RUN dotnet restore "Otto.products/Otto.products.csproj"
COPY . .
WORKDIR "/src/Otto.products"
RUN dotnet build "Otto.products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otto.products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Otto.products.dll"]