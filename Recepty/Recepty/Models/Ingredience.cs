namespace Recepty.Models;

public class Ingredience
{
    public int Id { get; set; }
    public int ReceptId { get; set; }
    public string Nazev { get; set; } = string.Empty;
    public string? Mnozstvi { get; set; }
    public string? Jednotka { get; set; }
}