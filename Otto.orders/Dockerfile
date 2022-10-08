#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otto.orders/Otto.orders.csproj", "Otto.orders/"]
COPY ["Otto.models/Otto.models.csproj", "Otto.models/"]
RUN dotnet restore "Otto.orders/Otto.orders.csproj"
COPY . .
WORKDIR "/src/Otto.orders"
RUN dotnet build "Otto.orders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otto.orders.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Otto.orders.dll"]