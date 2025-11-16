FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AuctionSystem.sln ./

COPY Services/Files/Files/Files.csproj             Services/Files/Files/
COPY Services/Shared/Shared.Base/Shared.Base.csproj                 Services/Shared/Shared.Base/
COPY Services/Shared/Shared.Events/Shared.Events.csproj             Services/Shared/Shared.Events/
COPY Services/Shared/Shared.Cache/Shared.Cache.csproj             Services/Shared/Shared.Cache/

RUN dotnet restore "Services/Files/Files/Files.csproj"

COPY . .
RUN dotnet publish "Services/Files/Files/Files.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

RUN apt-get update && apt-get install -y libfontconfig1 libfreetype6
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Files.dll"]