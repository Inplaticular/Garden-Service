# 1. Build application in image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY ["Src/Garden-Service.WebAPI/Garden-Service.WebAPI.csproj", "Garden-Service.WebAPI/"]
COPY ["Src/Garden-Service.Infrastructure/Garden-Service.Infrastructure.csproj", "Garden-Service.Infrastructure/"]
COPY ["Src/Garden-Service.Core/Garden-Service.Core.csproj", "Garden-Service.Core/"]

RUN dotnet restore "Garden-Service.WebAPI/Garden-Service.WebAPI.csproj"

COPY ./Src .
WORKDIR "/src/Garden-Service.WebAPI"

RUN dotnet build "Garden-Service.WebAPI.csproj" -c Release -o /app/build

# 2. Publish built application in image
FROM build AS publish
RUN dotnet publish "Garden-Service.WebAPI.csproj" -c Release -o /app/publish

# 3. Take published version
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS final
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .

