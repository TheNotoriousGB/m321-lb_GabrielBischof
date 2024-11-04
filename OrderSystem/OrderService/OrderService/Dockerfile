# Verwende das offizielle .NET SDK-Image für den Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Setze das Arbeitsverzeichnis
WORKDIR /app

# Kopiere die csproj-Datei und restore die Abhängigkeiten
COPY ./OrderService/OrderService.csproj ./OrderService/
RUN dotnet restore ./OrderService/OrderService.csproj

# Kopiere den gesamten Quellcode und baue die Anwendung
COPY ./OrderService/. ./OrderService/
WORKDIR /app/OrderService
RUN dotnet publish -c Release -o out

# Verwende das offizielle .NET Runtime-Image für die Ausführung
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/OrderService/out .

# Setze den Befehl zum Starten der Anwendung
ENTRYPOINT ["dotnet", "OrderService.dll"]
