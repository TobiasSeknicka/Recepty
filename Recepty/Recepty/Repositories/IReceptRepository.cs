using System.Collections.Generic;
using Recepty.Models;

namespace Recepty.Repositories;

public interface IReceptRepository
{
    IEnumerable<Recept> GetAll();
    Recept? GetById(int id);
    void Add(Recept recept);
    void Update(Recept recept);
    void Delete(int id);
    IEnumerable<Kategorie> GetKategorie();
}