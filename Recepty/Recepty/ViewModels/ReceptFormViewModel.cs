using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recepty.Models;
using Recepty.Repositories;

namespace Recepty.ViewModels;

public partial class ReceptFormViewModel : ViewModelBase
{
    private readonly IReceptRepository _repository;
    private readonly Action _onSaved;
    private readonly Recept? _editingRecept;

    [ObservableProperty]
    private string _nazev = string.Empty;

    [ObservableProperty]
    private string _postup = string.Empty;

    [ObservableProperty]
    private string _pocetPorci = string.Empty;

    [ObservableProperty]
    private Kategorie? _selectedKategorie;

    [ObservableProperty]
    private string _chyba = string.Empty;

    public ObservableCollection<Kategorie> Kategorie { get; } = new();

    public string Nadpis => _editingRecept == null ? "Nový recept" : "Úprava receptu";

    public ReceptFormViewModel(IReceptRepository repository, Action onSaved, Recept? editingRecept)
    {
        _repository = repository;
        _onSaved = onSaved;
        _editingRecept = editingRecept;

        foreach (var k in _repository.GetKategorie())
            Kategorie.Add(k);

        if (_editingRecept != null)
        {
            Nazev = _editingRecept.Nazev;
            Postup = _editingRecept.Postup ?? string.Empty;
            PocetPorci = _editingRecept.PocetPorci?.ToString() ?? string.Empty;
            SelectedKategorie = Kategorie.FirstOrDefault(k => k.Id == _editingRecept.KategorieId);
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Nazev))
        {
            Chyba = "Název je povinný.";
            return;
        }
        if (SelectedKategorie == null)
        {
            Chyba = "Vyber kategorii.";
            return;
        }

        int? porci = null;
        if (!string.IsNullOrWhiteSpace(PocetPorci))
        {
            if (!int.TryParse(PocetPorci, out var p) || p <= 0)
            {
                Chyba = "Počet porcí musí být kladné číslo.";
                return;
            }
            porci = p;
        }

        if (_editingRecept == null)
        {
            _repository.Add(new Recept
            {
                Nazev = Nazev,
                Postup = string.IsNullOrWhiteSpace(Postup) ? null : Postup,
                PocetPorci = porci,
                KategorieId = SelectedKategorie.Id
            });
        }
        else
        {
            _repository.Update(new Recept
            {
                Id = _editingRecept.Id,
                Nazev = Nazev,
                Postup = string.IsNullOrWhiteSpace(Postup) ? null : Postup,
                PocetPorci = porci,
                KategorieId = SelectedKategorie.Id
            });
        }
        _onSaved();
    }

    [RelayCommand]
    private void Cancel() => _onSaved();
}