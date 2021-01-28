#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TestChat.Api/TestChat.Api.csproj", "TestChat.Api/"]
COPY ["TestChat.Core/TestChat.Core.csproj", "TestChat.Core/"]
COPY ["TestChat.Services/TestChat.Services.csproj", "TestChat.Services/"]
COPY ["TestChat.Data/TestChat.Data.csproj", "TestChat.Data/"]
RUN dotnet restore "TestChat.Api/TestChat.Api.csproj"
COPY . .
WORKDIR "/src/TestChat.Api"
RUN dotnet build "TestChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestChat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestChat.Api.dll"]