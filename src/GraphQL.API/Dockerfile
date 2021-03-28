#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/GraphQL.API/GraphQL.API.csproj", "src/GraphQL.API/"]
COPY ["src/GraphQL.Infrastructure/GraphQL.Infrastructure.csproj", "src/GraphQL.Infrastructure/"]
COPY ["src/GraphQL.Core/GraphQL.Core.csproj", "src/GraphQL.Core/"]
RUN dotnet restore "src/GraphQL.API/GraphQL.API.csproj"
COPY . .
WORKDIR "/src/src/GraphQL.API"
RUN dotnet build "GraphQL.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQL.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQL.API.dll"]
