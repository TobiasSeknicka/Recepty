namespace Recepty.Models;

public class Recept
{
    public int Id { get; set; }
    public string Nazev { get; set; } = string.Empty;
    public string? Postup { get; set; }
    public int? PocetPorci { get; set; }
    public int KategorieId { get; set; }
    public string KategorieNazev { get; set; } = string.Empty;
}