using CommunityToolkit.Mvvm.ComponentModel;
using Recepty.Repositories;
using Recepty.Models;

namespace Recepty.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IReceptRepository _receptRepository;
    private readonly IIngredRepository _ingredRepository;

    [ObservableProperty]
    private ViewModelBase _currentPage = null!;

    public MainWindowViewModel(IReceptRepository receptRepository, IIngredRepository ingredRepository)
    {
        _receptRepository = receptRepository;
        _ingredRepository = ingredRepository;
        ShowList();
    }

    public void ShowList()
    {
        CurrentPage = new ReceptListViewModel(
            _receptRepository,
            recept => ShowDetail(recept),
            recept => ShowForm(recept)
        );
    }

    private void ShowDetail(Recept recept)
    {
        CurrentPage = new ReceptDetailViewModel(
            _ingredRepository,
            recept,
            onBack: ShowList
        );
    }

    private void ShowForm(Recept? recept)
    {
        CurrentPage = new ReceptFormViewModel(
            _receptRepository,
            onSaved: ShowList,
            editingRecept: recept
        );
    }
}