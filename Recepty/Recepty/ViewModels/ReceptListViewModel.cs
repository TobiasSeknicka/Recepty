using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recepty.Models;
using Recepty.Repositories;

namespace Recepty.ViewModels;

public partial class ReceptListViewModel : ViewModelBase
{
    private readonly IReceptRepository _repository;
    private readonly Action<Recept> _showDetail;
    private readonly Action<Recept?> _showForm;

    private List<Recept> _allRecepty = new();

    public ObservableCollection<Recept> Recepty { get; } = new();

    [ObservableProperty]
    private Recept? _selectedRecept;

    [ObservableProperty]
    private string _hledani = string.Empty;

    public ReceptListViewModel(IReceptRepository repository, Action<Recept> showDetail, Action<Recept?> showForm)
    {
        _repository = repository;
        _showDetail = showDetail;
        _showForm = showForm;
        LoadRecepty();
    }

    private void LoadRecepty()
    {
        _allRecepty = _repository.GetAll().ToList();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Recepty.Clear();
        var filtrovane = _allRecepty.Where(r =>
            string.IsNullOrWhiteSpace(Hledani) ||
            r.Nazev.Contains(Hledani, StringComparison.OrdinalIgnoreCase));
        foreach (var r in filtrovane)
            Recepty.Add(r);
    }

    partial void OnHledaniChanged(string value) => ApplyFilter();

    [RelayCommand]
    private void AddRecept() => _showForm(null);

    [RelayCommand(CanExecute = nameof(IsReceptSelected))]
    private void ShowDetail() => _showDetail(SelectedRecept!);

    [RelayCommand(CanExecute = nameof(IsReceptSelected))]
    private void DeleteRecept()
    {
        _repository.Delete(SelectedRecept!.Id);
        _allRecepty.Remove(SelectedRecept);
        Recepty.Remove(SelectedRecept);
        SelectedRecept = null;
    }

    private bool IsReceptSelected() => SelectedRecept != null;

    partial void OnSelectedReceptChanged(Recept? value)
    {
        ShowDetailCommand.NotifyCanExecuteChanged();
        DeleteReceptCommand.NotifyCanExecuteChanged();
    }
}