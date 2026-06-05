CREATE TABLE IF NOT EXISTS kategorie (
                                         id    SERIAL PRIMARY KEY,
                                         nazev TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS recept (
                                      id           SERIAL PRIMARY KEY,
                                      nazev        TEXT NOT NULL,
                                      postup       TEXT,
                                      pocet_porci  INT,
                                      kategorie_id INT NOT NULL REFERENCES kategorie(id)
    );

CREATE TABLE IF NOT EXISTS ingredience (
                                           id        SERIAL PRIMARY KEY,
                                           recept_id INT  NOT NULL REFERENCES recept(id) ON DELETE CASCADE,
    nazev     TEXT NOT NULL,
    mnozstvi  TEXT,
    jednotka  TEXT
    );