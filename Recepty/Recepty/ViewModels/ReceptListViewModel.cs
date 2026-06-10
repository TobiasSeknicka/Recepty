using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public ObservableCollection<Recept> Recepty { get; } = new();

    [ObservableProperty]
    private Recept? _selectedRecept;

    public ReceptListViewModel(IReceptRepository repository, Action<Recept> showDetail, Action<Recept?> showForm)
    {
        _repository = repository;
        _showDetail = showDetail;
        _showForm = showForm;
        LoadRecepty();
    }

    private void LoadRecepty()
    {
        Recepty.Clear();
        foreach (var r in _repository.GetAll())
            Recepty.Add(r);
    }

    [RelayCommand]
    private void AddRecept() => _showForm(null);

    [RelayCommand(CanExecute = nameof(IsReceptSelected))]
    private void ShowDetail() => _showDetail(SelectedRecept!);

    [RelayCommand(CanExecute = nameof(IsReceptSelected))]
    private void DeleteRecept()
    {
        _repository.Delete(SelectedRecept!.Id);
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