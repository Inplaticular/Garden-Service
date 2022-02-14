FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Garden-Service.WebAPI/Garden-Service.WebAPI.csproj", "Garden-Service.WebAPI/"]
RUN dotnet restore "src/Garden-Service.WebAPI/Garden-Service.WebAPI.csproj"
COPY . .
WORKDIR "/src/Garden-Service.WebAPI"
RUN dotnet build "Garden-Service.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Garden-Service.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Garden-Service.WebAPI.dll"]
