INSERT INTO kategorie (nazev) VALUES
                                  ('Snídaně'), ('Oběd'), ('Večeře'), ('Dezert')
    ON CONFLICT (nazev) DO NOTHING;