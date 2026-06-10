using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recepty.Models;
using Recepty.Repositories;

namespace Recepty.ViewModels;

public partial class ReceptDetailViewModel : ViewModelBase
{
    private readonly IIngredRepository _ingredRepository;
    private readonly Recept _recept;

    public string ReceptNazev => _recept.Nazev;
    public string ReceptKategorie => _recept.KategorieNazev;
    public string ReceptPostup => string.IsNullOrWhiteSpace(_recept.Postup) ? "(bez postupu)" : _recept.Postup;

    public ObservableCollection<Ingredience> Ingredience { get; } = new();

    [ObservableProperty]
    private Ingredience? _selectedIngredience;

    // pole formuláře pro novou/upravovanou ingredienci
    [ObservableProperty]
    private string _ingredNazev = string.Empty;

    [ObservableProperty]
    private string _ingredMnozstvi = string.Empty;

    [ObservableProperty]
    private string _ingredJednotka = string.Empty;

    [ObservableProperty]
    private string _chyba = string.Empty;

    private readonly Action _onBack;

    public ReceptDetailViewModel(IIngredRepository ingredRepository, Recept recept, Action onBack)
    {
        _ingredRepository = ingredRepository;
        _recept = recept;
        _onBack = onBack;
        LoadIngredience();
    }

    private void LoadIngredience()
    {
        Ingredience.Clear();
        foreach (var i in _ingredRepository.GetByReceptId(_recept.Id))
            Ingredience.Add(i);
    }

    [RelayCommand]
    private void AddIngredience()
    {
        if (string.IsNullOrWhiteSpace(IngredNazev))
        {
            Chyba = "Název ingredience je povinný.";
            return;
        }

        var nova = new Ingredience
        {
            ReceptId = _recept.Id,
            Nazev = IngredNazev,
            Mnozstvi = string.IsNullOrWhiteSpace(IngredMnozstvi) ? null : IngredMnozstvi,
            Jednotka = string.IsNullOrWhiteSpace(IngredJednotka) ? null : IngredJednotka
        };
        _ingredRepository.Add(nova);
        LoadIngredience();
        ClearForm();
    }

    [RelayCommand(CanExecute = nameof(IsIngredienceSelected))]
    private void UpdateIngredience()
    {
        if (string.IsNullOrWhiteSpace(IngredNazev))
        {
            Chyba = "Název ingredience je povinný.";
            return;
        }

        SelectedIngredience!.Nazev = IngredNazev;
        SelectedIngredience.Mnozstvi = string.IsNullOrWhiteSpace(IngredMnozstvi) ? null : IngredMnozstvi;
        SelectedIngredience.Jednotka = string.IsNullOrWhiteSpace(IngredJednotka) ? null : IngredJednotka;
        _ingredRepository.Update(SelectedIngredience);
        LoadIngredience();
        ClearForm();
    }

    [RelayCommand(CanExecute = nameof(IsIngredienceSelected))]
    private void DeleteIngredience()
    {
        _ingredRepository.Delete(SelectedIngredience!.Id);
        LoadIngredience();
        ClearForm();
    }

    [RelayCommand]
    private void Back() => _onBack();

    private void ClearForm()
    {
        IngredNazev = string.Empty;
        IngredMnozstvi = string.Empty;
        IngredJednotka = string.Empty;
        Chyba = string.Empty;
        SelectedIngredience = null;
    }

    private bool IsIngredienceSelected() => SelectedIngredience != null;

    partial void OnSelectedIngredienceChanged(Ingredience? value)
    {
        UpdateIngredienceCommand.NotifyCanExecuteChanged();
        DeleteIngredienceCommand.NotifyCanExecuteChanged();
        if (value != null)
        {
            IngredNazev = value.Nazev;
            IngredMnozstvi = value.Mnozstvi ?? string.Empty;
            IngredJednotka = value.Jednotka ?? string.Empty;
        }
    }
}