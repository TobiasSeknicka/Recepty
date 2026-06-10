# Recepty

Desktopová aplikace na správu receptů a jejich ingrediencí. Postavená v Avalonii (MVVM) nad PostgreSQL databází běžící v Dockeru.

## Co aplikace umí
- Seznam receptů (přidání, mazání, otevření detailu)
- Detail receptu s plným CRUD nad ingrediencemi
- Formulář pro vytvoření a úpravu receptu s výběrem kategorie a validací

## Databáze
Tři tabulky:
- `recept` — hlavní entita
- `ingredience` — dětská entita (vztah 1:N: jeden recept má více ingrediencí)
- `kategorie` — číselník (snídaně / oběd / večeře / dezert)

## Požadavky
- Docker Desktop
- .NET 10 SDK
- JetBrains Rider (nebo jiné IDE pro .NET)

## Jak spustit

### 1. Naklonuj repozitář
git clone https://github.com/TobiasSeknicka/Recepty.git

### 2. Vytvoř soubor .env
Ve složce, kde leží `docker-compose.yaml` (vedle `Program.cs`), zkopíruj `.env.example` jako `.env` a vyplň hodnoty:

HOST=localhost
PORT=5432
DATABASE=recepty_db
USERNAME=recepty_user
PASSWORD=tvoje_heslo

### 3. Spusť databázi
Spusť Docker Desktop. Pak v terminálu přejdi do složky s `docker-compose.yaml` a spusť:

docker compose up -d

Tím se nastartuje PostgreSQL, vytvoří se tabulky (`schema.sql`) a naplní se číselník kategorií (`seed.sql`).

### 4. Spusť aplikaci
Otevři `Recepty.sln` v Rideru a spusť tlačítkem ▶ (nebo `dotnet run` ze složky projektu).

## Poznámky
- Soubor `.env` obsahuje heslo k databázi a není verzovaný v gitu. Slouží k tomu `.env.example` jako šablona.
- Data jsou uložená v Docker volume, takže přežijí restart aplikace i kontejneru.