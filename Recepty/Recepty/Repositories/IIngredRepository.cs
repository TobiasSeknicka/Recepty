using System.Collections.Generic;
using Recepty.Models;

namespace Recepty.Repositories;

public interface IIngredRepository
{
    IEnumerable<Ingredience> GetByReceptId(int receptId);
    void Add(Ingredience ingredience);
    void Update(Ingredience ingredience);
    void Delete(int id);
}