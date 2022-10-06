#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Otto.m.tokens/Otto.m.tokens.csproj", "Otto.m.tokens/"]
RUN dotnet restore "Otto.m.tokens/Otto.m.tokens.csproj"
COPY . .
WORKDIR "/src/Otto.m.tokens"
RUN dotnet build "Otto.m.tokens.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Otto.m.tokens.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Otto.m.tokens.dll"]