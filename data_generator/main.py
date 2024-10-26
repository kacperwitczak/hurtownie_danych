import pyodbc
from faker import Faker
import random
import time

start_time = time.time()

# Initialize data generator
fake = Faker('pl_PL')

# Database connection string
conn_str = (
    'DRIVER={ODBC Driver 17 for SQL Server};'
    'SERVER=laptopkacper;'
    'DATABASE=Kasyno;'
    'Trusted_Connection=yes;'
)
conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

# Define the number of entries to generate
num_entries = 10000

# Clear tables at the beginning of the script
def clear_tables():
    tables = [
        "Transakcje",       # 1st
        "Rozgrywki",        # 2nd
        "UstawienieStolu",  # 3rd
        "Stoly",            # 4th
        "Krupierzy",        # 5th
        "TypGry",           # 6th
        "Lokalizacje",      # 7th
        "TypTransakcji"     # 8th
    ]

    for table in tables:
        cursor.execute(f"DELETE FROM {table}")
    
    print("All tables cleared.")

# Function to generate data and insert it into tables
def generate_data():

    # Dealers
    dealers_data = []
    for i in range(num_entries):
        imie = fake.first_name()
        nazwisko = fake.last_name()
        pesel = fake.random_number(digits=11, fix_len=True)
        poczatek_pracy = fake.date_between(start_date='-5y', end_date='today')
        dealers_data.append((i + 1, imie, nazwisko, pesel, poczatek_pracy))

    cursor.executemany("""
        INSERT INTO Krupierzy (ID, Imie, Nazwisko, Pesel, PoczatekPracy)
        VALUES (?, ?, ?, ?, ?)
    """, dealers_data)
    print("Dealers done")

    # Game Types
    gry = ["Poker", "Ruletka", "Blackjack", "Baccarat", "Craps"]
    game_types_data = [(i + 1, gra) for i, gra in enumerate(gry)]
    cursor.executemany("""
        INSERT INTO TypGry (ID, NazwaGry)
        VALUES (?, ?)
    """, game_types_data)
    print("Game types done")

    # Tables
    tables_data = []
    for i in range(num_entries):
        typ_gry_id = random.randint(1, len(gry))
        minimalna_stawka = round(random.uniform(1, 100), 2)
        maksymalna_stawka = round(minimalna_stawka + random.uniform(50, 500), 2)
        liczba_miejsc = random.randint(2, 10)
        tables_data.append((i + 1, typ_gry_id, minimalna_stawka, maksymalna_stawka, liczba_miejsc))

    cursor.executemany("""
        INSERT INTO Stoly (ID, FK_TypGryID, MinimalnaStawka, MaksymalnaStawka, LiczbaMiejsc)
        VALUES (?, ?, ?, ?, ?)
    """, tables_data)
    print("Tables done")

    # Locations
    locations = {}
    locations_data = []
    for i in range(num_entries):
        while True:
            pietro = random.randint(0, num_entries)
            rzad = random.randint(1, num_entries)
            kolumna = random.randint(1, num_entries)
            location_id = (pietro, rzad, kolumna)

            # Ensure no duplicate locations
            if location_id not in locations:
                locations[location_id] = True
                locations_data.append((i + 1, pietro, rzad, kolumna))
                break

    cursor.executemany("""
        INSERT INTO Lokalizacje (ID, Pietro, Rzad, Kolumna)
        VALUES (?, ?, ?, ?)
    """, locations_data)
    print("Locations done")

    # Table Settings
    table_settings_data = []
    used_locations = set()
    for i in range(num_entries):
        data_start = fake.date_between(start_date='-1y', end_date='today')
        data_koniec = None if random.choice([True, False]) else fake.date_between(start_date=data_start, end_date='today')
        stol_id = random.randint(1, num_entries)

        # Ensure that a table is set in a unique location
        while True:
            lokalizacja_id = random.randint(1, num_entries)
            if lokalizacja_id not in used_locations:
                used_locations.add(lokalizacja_id)
                table_settings_data.append((i + 1, data_start, data_koniec, stol_id, lokalizacja_id))
                break

    cursor.executemany("""
        INSERT INTO UstawienieStolu (ID, DataStart, DataKoniec, FK_StolID, FK_LokalizacjaID)
        VALUES (?, ?, ?, ?, ?)
    """, table_settings_data)
    print("Table settings done")

    # Games
    games_data = []
    for i in range(num_entries):
        krupier_id = random.randint(1, num_entries)
        ustawienie_stolu_id = random.randint(1, num_entries)
        data_start = fake.date_between(start_date='-1y', end_date='today')
        data_koniec = fake.date_between(start_date=data_start, end_date='today')
        
        games_data.append((i + 1, krupier_id, ustawienie_stolu_id, data_start, data_koniec))

    cursor.executemany("""
        INSERT INTO Rozgrywki (ID, FK_KrupierID, FK_UstawienieStoluID, DataStart, DataKoniec)
        VALUES (?, ?, ?, ?, ?)
    """, games_data)
    print("Games done")

    # Transaction Types
    typy_transakcji = ["Wplata", "Wyplata"]
    transaction_types_data = [(i + 1, typ) for i, typ in enumerate(typy_transakcji)]
    cursor.executemany("""
        INSERT INTO TypTransakcji (ID, Typ)
        VALUES (?, ?)
    """, transaction_types_data)
    print("Transaction types done")

    # Transactions
    transactions_data = []
    for i in range(2 * num_entries):  # Allow more transactions than entries
        rozgrywka_id = random.randint(1, num_entries)
        typ_transakcji_id = random.randint(1, len(typy_transakcji))
        kwota = round(random.uniform(10, 10000), 2)
        
        transactions_data.append((i + 1, rozgrywka_id, typ_transakcji_id, kwota))

    cursor.executemany("""
        INSERT INTO Transakcje (ID, FK_RozgrywkaID, FK_TypTransakcjiID, Kwota)
        VALUES (?, ?, ?, ?)
    """, transactions_data)
    print("Transactions done")

clear_tables()

# Insert data
generate_data()

# Commit changes and close connection
conn.commit()
cursor.close()
conn.close()

end_time = time.time()
execution_time = end_time - start_time  # Oblicza czas wykonania
print(f"Czas wykonania: {execution_time:.6f} sekund")
