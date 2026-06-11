# Recepty

Desktopová aplikace na správu receptů a jejich ingrediencí. Postavená v Avalonii (MVVM) nad PostgreSQL databází běžící v Dockeru.

## O projektu

Aplikace slouží k evidenci vlastních receptů — vytváření receptů s kategorií, počtem porcí a postupem, a správa jejich ingrediencí s množstvím a jednotkou.

Projekt je postavený na architektuře MVVM v Avalonii. Datový přístup je oddělený přes repository pattern, závislosti spravuje Dependency Injection a připojení k databázi se konfiguruje přes `.env`.

## Funkce

- Seznam receptů s vyhledáváním podle názvu
- Plný CRUD nad recepty i ingrediencemi
- Formulář s ComboBoxem napojeným na číselník kategorií a validací vstupů
- Cascade delete — smazání receptu odstraní i jeho ingredience

## Databáze

Tři tabulky:
- `recept` — hlavní entita
- `ingredience` — dětská entita (vztah 1:N: jeden recept má více ingrediencí)
- `kategorie` — číselník (snídaně / oběd / večeře / dezert)

Schéma se vytvoří automaticky při prvním startu kontejneru ze souboru `schema.sql`, číselník se naplní ze `seed.sql`. Data jsou uložená v Docker volume, takže přežijí restart aplikace i kontejneru.

## Požadavky

- Docker Desktop
- .NET 10 SDK
- JetBrains Rider (nebo jiné IDE pro .NET)

## Spuštění projektu

### 1. Klonování repozitáře
git clone https://github.com/TobiasSeknicka/Recepty.git

### 2. Vytvoření souboru `.env`

Ve složce, kde leží `docker-compose.yaml`, je potřeba zkopírovat `.env.example` jako `.env` a vyplnit hodnoty:
HOST=localhost
PORT=5432
DATABASE=recepty_db
USERNAME=recepty_user
PASSWORD=tvoje_heslo

Soubor `.env` obsahuje skutečné heslo k databázi a není verzovaný v gitu. Šablonou je `.env.example`.

### 3. Spuštění databáze

Po spuštění Docker Desktopu stačí v terminálu (ve složce s `docker-compose.yaml`) zadat:
docker compose up -d
Tím se nastartuje PostgreSQL, vytvoří se tabulky a naplní se číselník kategorií.

### 4. Spuštění aplikace

Otevřít `Recepty.sln` v Rideru a spustit tlačítkem ▶ (případně příkazem `dotnet run` ze složky projektu).