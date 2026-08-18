FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RunningRacesApi/RunningRacesApi.csproj", "RunningRacesApi/"]
RUN dotnet restore "RunningRacesApi/RunningRacesApi.csproj"
COPY . .
WORKDIR "/src/RunningRacesApi"
RUN dotnet build "RunningRacesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RunningRacesApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RunningRacesApi.dll"]