using Recepty.Repositories;

namespace Recepty.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IReceptRepository _receptRepository;
    private readonly IIngredRepository _ingredRepository;

    public MainWindowViewModel(IReceptRepository receptRepository, IIngredRepository ingredRepository)
    {
        _receptRepository = receptRepository;
        _ingredRepository = ingredRepository;
    }
}