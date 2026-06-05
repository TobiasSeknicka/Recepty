using DotNetEnv;
using Recepty.Repositories;
using Recepty.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Recepty;

public class Services
{
    public static ServiceProvider ServiceCollection()
    {
        Env.TraversePath().Load();
        var services = new ServiceCollection();

        services.AddTransient<MainWindowViewModel>();

        string host     = Env.GetString("HOST");
        string port     = Env.GetString("PORT");
        string database = Env.GetString("DATABASE");
        string user     = Env.GetString("USERNAME");
        string pass     = Env.GetString("PASSWORD");
        var connectionString =
            $"Host={host};Port={port};Database={database};Username={user};Password={pass}";

        services.AddSingleton<IReceptRepository>(new ReceptRepository(connectionString));
        services.AddSingleton<IIngredRepository>(new IngredRepository(connectionString));

        return services.BuildServiceProvider();
    }
}