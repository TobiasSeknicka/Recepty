using System;
using System.Collections.Generic;
using Npgsql;
using Recepty.Models;

namespace Recepty.Repositories;

public class ReceptRepository : IReceptRepository
{
    private readonly NpgsqlConnection _connection;

    public ReceptRepository(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public IEnumerable<Recept> GetAll()
    {
        var items = new List<Recept>();
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT r.id, r.nazev, r.postup, r.pocet_porci, r.kategorie_id, k.nazev " +
            "FROM recept r JOIN kategorie k ON r.kategorie_id = k.id",
            _connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new Recept
            {
                Id           = reader.GetInt32(0),
                Nazev        = reader.GetString(1),
                Postup       = reader.IsDBNull(2) ? null : reader.GetString(2),
                PocetPorci   = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                KategorieId  = reader.GetInt32(4),
                KategorieNazev = reader.GetString(5)
            });
        }
        _connection.Close();
        return items;
    }

    public Recept? GetById(int id)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT r.id, r.nazev, r.postup, r.pocet_porci, r.kategorie_id, k.nazev " +
            "FROM recept r JOIN kategorie k ON r.kategorie_id = k.id WHERE r.id = @id",
            _connection);
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        Recept? result = null;
        if (reader.Read())
        {
            result = new Recept
            {
                Id           = reader.GetInt32(0),
                Nazev        = reader.GetString(1),
                Postup       = reader.IsDBNull(2) ? null : reader.GetString(2),
                PocetPorci   = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                KategorieId  = reader.GetInt32(4),
                KategorieNazev = reader.GetString(5)
            };
        }
        _connection.Close();
        return result;
    }

    public void Add(Recept r)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO recept (nazev, postup, pocet_porci, kategorie_id) " +
            "VALUES (@nazev, @postup, @pocetPorci, @kategorieId)",
            _connection);
        cmd.Parameters.AddWithValue("nazev",      r.Nazev);
        cmd.Parameters.AddWithValue("postup",     (object?)r.Postup     ?? DBNull.Value);
        cmd.Parameters.AddWithValue("pocetPorci", (object?)r.PocetPorci ?? DBNull.Value);
        cmd.Parameters.AddWithValue("kategorieId", r.KategorieId);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Update(Recept r)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "UPDATE recept SET nazev=@nazev, postup=@postup, " +
            "pocet_porci=@pocetPorci, kategorie_id=@kategorieId WHERE id=@id",
            _connection);
        cmd.Parameters.AddWithValue("id",         r.Id);
        cmd.Parameters.AddWithValue("nazev",      r.Nazev);
        cmd.Parameters.AddWithValue("postup",     (object?)r.Postup     ?? DBNull.Value);
        cmd.Parameters.AddWithValue("pocetPorci", (object?)r.PocetPorci ?? DBNull.Value);
        cmd.Parameters.AddWithValue("kategorieId", r.KategorieId);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Delete(int id)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM recept WHERE id=@id", _connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public IEnumerable<Kategorie> GetKategorie()
    {
        var items = new List<Kategorie>();
        _connection.Open();
        using var cmd = new NpgsqlCommand("SELECT id, nazev FROM kategorie ORDER BY id", _connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            items.Add(new Kategorie { Id = reader.GetInt32(0), Nazev = reader.GetString(1) });
        _connection.Close();
        return items;
    }
}