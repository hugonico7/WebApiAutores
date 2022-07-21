FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebAPIAutores/WebAPIAutores.csproj", "WebAPIAutores/"]
RUN dotnet restore "WebAPIAutores/WebAPIAutores.csproj"
COPY . .
WORKDIR "/src/WebAPIAutores"
RUN dotnet build "WebAPIAutores.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPIAutores.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPIAutores.dll"]
