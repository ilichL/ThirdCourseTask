using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThirdCourseTask.Infrastructure.Database;
using ThirdCourseTask.Infrastructure.Database.Repositories.Abstractions;
using ThirdCourseTask.Infrastructure.Database.Repositories.Implementations;
using ThirdCourseTask.Infrastructure.Services.Abstractions;
using ThirdCourseTask.Infrastructure.Services.Implementations;
using ThirdCourseTask.UI;

Console.WriteLine("Select a database:");
Console.WriteLine("1) SQL Server");
Console.WriteLine("2) PostgreSQL");

string? choice = null;

while (choice != "1" && choice != "2")
{
    Console.Write("Enter 1 or 2: ");
    choice = Console.ReadLine();
}

if (choice == "2")
{
    Environment.SetEnvironmentVariable("DB_PROVIDER", "Postgres");
}
else
{
    Environment.SetEnvironmentVariable("DB_PROVIDER", "SqlServer");
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        cfg.AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddSingleton<IInputService, ConsoleInputService>();
    })
    .Build();

var inputService = host.Services.GetRequiredService<IInputService>();

while (true)
{
    try
    {
        await inputService.RunAsync();
    }
    catch (ApplicationException ex)
    {
        Console.WriteLine("Application error: " + ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine("Operation error: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Unexpected error: " + ex.Message);
    }
}
