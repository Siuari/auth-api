#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuthApi.Api/AuthApi.Api.csproj", "AuthApi.Api/"]
RUN dotnet restore "AuthApi.Api/AuthApi.Api.csproj"
COPY . .
WORKDIR "/src/AuthApi.Api"
RUN dotnet build "AuthApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthApi.Api.dll"]