using System;
using System.Collections.Generic;
using Npgsql;
using Recepty.Models;

namespace Recepty.Repositories;

public class IngredRepository : IIngredRepository
{
    private readonly NpgsqlConnection _connection;

    public IngredRepository(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public IEnumerable<Ingredience> GetByReceptId(int receptId)
    {
        var items = new List<Ingredience>();
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT id, recept_id, nazev, mnozstvi, jednotka FROM ingredience WHERE recept_id=@receptId",
            _connection);
        cmd.Parameters.AddWithValue("receptId", receptId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new Ingredience
            {
                Id        = reader.GetInt32(0),
                ReceptId  = reader.GetInt32(1),
                Nazev     = reader.GetString(2),
                Mnozstvi  = reader.IsDBNull(3) ? null : reader.GetString(3),
                Jednotka  = reader.IsDBNull(4) ? null : reader.GetString(4)
            });
        }
        _connection.Close();
        return items;
    }

    public void Add(Ingredience i)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO ingredience (recept_id, nazev, mnozstvi, jednotka) " +
            "VALUES (@receptId, @nazev, @mnozstvi, @jednotka)",
            _connection);
        cmd.Parameters.AddWithValue("receptId", i.ReceptId);
        cmd.Parameters.AddWithValue("nazev",    i.Nazev);
        cmd.Parameters.AddWithValue("mnozstvi", (object?)i.Mnozstvi ?? DBNull.Value);
        cmd.Parameters.AddWithValue("jednotka", (object?)i.Jednotka ?? DBNull.Value);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Update(Ingredience i)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand(
            "UPDATE ingredience SET nazev=@nazev, mnozstvi=@mnozstvi, jednotka=@jednotka WHERE id=@id",
            _connection);
        cmd.Parameters.AddWithValue("id",       i.Id);
        cmd.Parameters.AddWithValue("nazev",    i.Nazev);
        cmd.Parameters.AddWithValue("mnozstvi", (object?)i.Mnozstvi ?? DBNull.Value);
        cmd.Parameters.AddWithValue("jednotka", (object?)i.Jednotka ?? DBNull.Value);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void Delete(int id)
    {
        _connection.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM ingredience WHERE id=@id", _connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
        _connection.Close();
    }
}