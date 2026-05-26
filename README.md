# SZL Backend API

Backend-API fuer das SZL-Projekt. Die Anwendung stellt REST-Endpunkte fuer Events, Laeuferinnen und Laeufer, Teams, Kategorien, Tags, Teilnahmen, Runden, Spenden, Geschenke, Ranglisten und Urkunden-PDFs bereit.

Das Projekt basiert auf ASP.NET Core, Entity Framework Core und PostgreSQL. Fuer die API-Dokumentation wird Swagger/OpenAPI verwendet, fuer die PDF-Erzeugung QuestPDF.

## Inhaltsverzeichnis

- [Features](#features)
- [Technologien](#technologien)
- [Projektstruktur](#projektstruktur)
- [Voraussetzungen](#voraussetzungen)
- [Konfiguration](#konfiguration)
- [Lokale Entwicklung](#lokale-entwicklung)
- [Docker](#docker)
- [API-Dokumentation](#api-dokumentation)
- [Wichtige Endpunkte](#wichtige-endpunkte)
- [Datenmodell](#datenmodell)
- [Sicherheit und sensible Daten](#sicherheit-und-sensible-daten)
- [Nützliche Befehle](#nützliche-befehle)

## Features

- Verwaltung von Events, Teams, Kategorien, Tags und Geschenken
- Verwaltung von Laeuferinnen und Laeufern
- Zuordnung von Teilnahmen zu Events, Teams, Kategorien, Tags und Personen
- Erfassung und Verwaltung von Rundenzeiten
- Spendenverwaltung pro Teilnahme
- Verwaltung von erhaltenen Geschenken
- Ranglisten fuer schnellste Teilnehmende nach Geschlecht
- Ranglisten fuer die meisten gueltigen Runden nach Geschlecht
- PDF-Generierung fuer Event-Urkunden
- Swagger UI fuer Entwicklung und manuelle API-Tests
- Dockerfile und Compose-Konfiguration fuer containerisierte Ausfuehrung

## Technologien

- .NET / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL via Npgsql
- Swagger / OpenAPI via Swashbuckle
- QuestPDF
- Docker

## Projektstruktur

```text
.
├── SZL_Backend.sln
├── compose.yaml
├── README.md
└── SZL_Backend/
    ├── Controllers/              # REST-Controller
    ├── Context/                  # Entity Framework DbContext
    ├── Dto/                      # Request- und Response-DTOs
    ├── Entities/                 # Datenbank-Entities
    ├── Assets/                   # Statische Assets, z. B. Logo fuer Urkunden
    ├── Properties/               # Lokale Startprofile
    ├── CertificatePdfRenderer.cs # PDF-Erzeugung fuer Urkunden
    ├── Program.cs                # Application Startup und Service-Konfiguration
    ├── Dockerfile
    └── SZL_Backend.csproj
```

## Voraussetzungen

Fuer die lokale Entwicklung werden benoetigt:

- .NET SDK passend zum Target Framework des Projekts
- PostgreSQL-Datenbank
- Optional: Docker und Docker Compose

Hinweis: Das Repository enthaelt keine Datenbank-Migrationen. Die Anwendung erwartet eine vorhandene PostgreSQL-Datenbank mit passendem Schema.

## Konfiguration

Die API benoetigt eine PostgreSQL-Verbindung unter dem Konfigurationsschluessel:

```text
ConnectionStrings:DefaultConnectionString
```

### Empfohlen: User Secrets fuer lokale Entwicklung

```bash
cd SZL_Backend
dotnet user-secrets set "ConnectionStrings:DefaultConnectionString" "<DEINE_POSTGRES_CONNECTION_STRING>"
```

Der Wert muss durch eine eigene lokale Verbindungszeichenfolge ersetzt werden. Keine echten Zugangsdaten in das Repository committen.

### Alternative: Umgebungsvariable

```bash
export ConnectionStrings__DefaultConnectionString="<DEINE_POSTGRES_CONNECTION_STRING>"
```

In Container-Umgebungen kann dieselbe Variable als Environment Variable gesetzt werden.

## Lokale Entwicklung

Repository klonen:

```bash
git clone <REPOSITORY_URL>
cd SZL_Backend
```

Dependencies wiederherstellen:

```bash
dotnet restore
```

Connection String konfigurieren:

```bash
cd SZL_Backend
dotnet user-secrets set "ConnectionStrings:DefaultConnectionString" "<DEINE_POSTGRES_CONNECTION_STRING>"
```

Projekt starten:

```bash
dotnet run
```

Im Development-Modus ist die Swagger UI standardmaessig im Root-Pfad der Anwendung erreichbar.

## Docker

Das Projekt enthaelt ein `Dockerfile` und eine `compose.yaml`.

Container bauen und starten:

```bash
docker compose up --build
```

Die Compose-Konfiguration veroeffentlicht die Anwendung standardmaessig auf:

```text
http://localhost:8080
```

Wichtig: Die Compose-Datei enthaelt bewusst keine Datenbank-Zugangsdaten. Fuer einen produktiven oder lokalen Containerbetrieb muss der Connection String sicher ueber Umgebungsvariablen, Secrets oder eine nicht versionierte lokale Override-Datei gesetzt werden.

Beispiel fuer eine Umgebungsvariable:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ASPNETCORE_URLS=http://+:8080
  - ConnectionStrings__DefaultConnectionString=<DEINE_POSTGRES_CONNECTION_STRING>
```

## API-Dokumentation

Swagger/OpenAPI ist im Development-Modus aktiviert.

Nach dem Start der Anwendung:

```text
http://localhost:<PORT>/
```

Die OpenAPI-Spezifikation liegt unter:

```text
/swagger/v1/swagger.json
```

## Wichtige Endpunkte

Die Controller verwenden das Muster:

```text
/api/<controller>
```

Die konfigurierte Route-Convention schreibt Controller-Tokens klein. Literal definierte Teilrouten bleiben in der Schreibweise erhalten, die im jeweiligen Controller angegeben ist.

### Stammdaten

| Bereich | Methode | Route | Beschreibung |
| --- | --- | --- | --- |
| Kategorien | GET | `/api/categories` | Alle Kategorien abrufen |
| Kategorien | GET | `/api/categories/{id}` | Kategorie nach ID abrufen |
| Kategorien | POST | `/api/categories` | Kategorie erstellen |
| Kategorien | PUT | `/api/categories/{id}` | Kategorie aktualisieren |
| Kategorien | DELETE | `/api/categories/{id}` | Kategorie loeschen |
| Events | GET | `/api/events` | Alle Events abrufen |
| Events | GET | `/api/events/{id}` | Event nach ID abrufen |
| Events | POST | `/api/events` | Event erstellen |
| Events | PUT | `/api/events/{id}` | Event aktualisieren |
| Events | DELETE | `/api/events/{id}` | Event loeschen |
| Teams | GET | `/api/teams` | Alle Teams abrufen |
| Teams | GET | `/api/teams/{id}` | Team nach ID abrufen |
| Teams | POST | `/api/teams` | Team erstellen |
| Teams | PUT | `/api/teams/{id}` | Team aktualisieren |
| Teams | DELETE | `/api/teams/{id}` | Team loeschen |
| Tags | GET | `/api/tags` | Alle Tags abrufen |
| Tags | GET | `/api/tags/{id}` | Tag nach ID abrufen |
| Tags | POST | `/api/tags` | Tag erstellen |
| Tags | PUT | `/api/tags/{id}` | Tag aktualisieren |
| Tags | DELETE | `/api/tags/{id}` | Tag loeschen |
| Geschenke | GET | `/api/gifts` | Alle Geschenke abrufen |
| Geschenke | GET | `/api/gifts/{id}` | Geschenk nach ID abrufen |
| Geschenke | POST | `/api/gifts` | Geschenk erstellen |
| Geschenke | PUT | `/api/gifts/{id}` | Geschenk aktualisieren |
| Geschenke | DELETE | `/api/gifts/{id}` | Geschenk loeschen |

### Teilnehmende und Runden

| Bereich | Methode | Route | Beschreibung |
| --- | --- | --- | --- |
| Laeufer | GET | `/api/runners` | Alle Laeuferinnen und Laeufer abrufen |
| Laeufer | GET | `/api/runners/{id}` | Person nach ID abrufen |
| Laeufer | POST | `/api/runners` | Person erstellen |
| Laeufer | PUT | `/api/runners/{id}` | Person aktualisieren |
| Laeufer | DELETE | `/api/runners/{id}` | Person loeschen |
| Teilnahmen | GET | `/api/participates` | Alle Teilnahmen abrufen |
| Teilnahmen | GET | `/api/participates/by-participateId/{id}` | Teilnahme nach ID abrufen |
| Teilnahmen | GET | `/api/participates/amount/by-teamId/{id}` | Anzahl der Teilnahmen eines Teams abrufen |
| Teilnahmen | GET | `/api/participates/by-tagId/{id}` | Teilnahme nach Tag abrufen |
| Teilnahmen | POST | `/api/participates` | Teilnahme erstellen |
| Teilnahmen | PUT | `/api/participates/{id}` | Teilnahme aktualisieren |
| Teilnahmen | DELETE | `/api/participates/{id}` | Teilnahme loeschen |
| Runden | GET | `/api/rounds` | Alle Runden abrufen |
| Runden | GET | `/api/rounds/by-roundId/{roundId}` | Runde nach Runden-ID abrufen |
| Runden | GET | `/api/rounds/by-participateId/{participateId}` | Runde nach Teilnahme-ID abrufen |
| Runden | GET | `/api/rounds/get-round-count/{participateId}` | Rundenanzahl einer Teilnahme abrufen |
| Runden | POST | `/api/rounds` | Runde erstellen |
| Runden | PUT | `/api/rounds/{id}` | Runde aktualisieren |
| Runden | DELETE | `/api/rounds/{id}` | Runde loeschen |

### Spenden, Geschenke, Ranglisten und Urkunden

| Bereich | Methode | Route | Beschreibung |
| --- | --- | --- | --- |
| Spenden | GET | `/api/donations` | Alle Spenden abrufen |
| Spenden | GET | `/api/donations/by-donation/{donationId}` | Spende nach ID abrufen |
| Spenden | GET | `/api/donations/by-participate/{participateId}` | Spenden einer Teilnahme abrufen |
| Spenden | POST | `/api/donations` | Spende erstellen |
| Spenden | PUT | `/api/donations/{id}` | Spende aktualisieren |
| Spenden | DELETE | `/api/donations/{id}` | Spende loeschen |
| Erhaltene Geschenke | GET | `/api/receives` | Alle Geschenk-Zuordnungen abrufen |
| Erhaltene Geschenke | GET | `/api/receives/{giftId}/{participateId}` | Geschenk-Zuordnung abrufen |
| Erhaltene Geschenke | GET | `/api/receives/gift/{giftId}` | Zuordnungen nach Geschenk abrufen |
| Erhaltene Geschenke | GET | `/api/receives/participate/{participateId}` | Zuordnungen nach Teilnahme abrufen |
| Erhaltene Geschenke | POST | `/api/receives` | Geschenk-Zuordnung erstellen |
| Erhaltene Geschenke | PUT | `/api/receives/{giftId}/{participateId}` | Geschenk-Zuordnung aktualisieren |
| Erhaltene Geschenke | DELETE | `/api/receives/{giftId}/{participateId}` | Geschenk-Zuordnung loeschen |
| Bestzeit | GET | `/api/besttime` | Bestzeiten abrufen |
| Bestzeit | GET | `/api/besttime/{participateId}` | Bestzeit einer Teilnahme abrufen |
| Leaderboard | GET | `/api/leaderboard/fastest-by-gender` | Schnellste Teilnehmende nach Geschlecht abrufen |
| Leaderboard | GET | `/api/leaderboard/most-rounds-by-gender` | Teilnehmende mit den meisten Runden nach Geschlecht abrufen |
| Urkunden | GET | `/api/certificates/{eventId}/pdf` | Urkunden-PDF fuer ein Event erzeugen |

## Datenmodell

Das Entity Framework Modell bildet unter anderem folgende Tabellen und Beziehungen ab:

- `runner`: Personen mit Name, Geschlecht und Geburtsdatum
- `event`: Veranstaltungen mit Name, Ort, Startzeit, Endzeit und Aktivstatus
- `team`: Teams
- `category`: Kategorien
- `tag`: Tags, zum Beispiel fuer Status oder Identifikation
- `participate`: Teilnahme einer Person an einem Event, optional mit Team, Kategorie und Tag
- `rounds`: Runden einer Teilnahme inklusive Zeitstempel und Gueltigkeitsstatus
- `donations`: Spenden pro Teilnahme
- `gifts`: Geschenke mit Voraussetzung
- `receives`: Zuordnung zwischen Teilnahme und Geschenk
- `BestTimeView`: Datenbank-View fuer Bestzeiten

## Sicherheit und sensible Daten

Dieses README enthaelt keine echten Zugangsdaten, Tokens, Passwoerter oder lokalen Secrets.

Bitte beachte:

- Keine echten Connection Strings in `README.md`, `compose.yaml`, `appsettings.json` oder andere versionierte Dateien schreiben.
- Lokale Zugangsdaten ueber `dotnet user-secrets`, Umgebungsvariablen oder sichere Secret Stores verwalten.
- Produktive Secrets nicht in Docker Images einbauen.
- Datenbank-Dumps, Zertifikate, private Schluessel und lokale Publish-Artefakte nicht committen.
- Vor Pull Requests pruefen, ob versehentlich sensible Daten geaendert oder hinzugefuegt wurden.

## Nützliche Befehle

Build ausfuehren:

```bash
dotnet build
```

Projekt starten:

```bash
dotnet run --project SZL_Backend/SZL_Backend.csproj
```

Docker Image bauen:

```bash
docker build -t szl_backend -f SZL_Backend/Dockerfile .
```

Docker Compose starten:

```bash
docker compose up --build
```

Docker Compose stoppen:

```bash
docker compose down
```

## Lizenz

Es ist aktuell keine Lizenzdatei im Repository enthalten. Falls das Projekt veroeffentlicht oder von anderen genutzt werden soll, sollte eine passende Lizenz ergaenzt werden.
