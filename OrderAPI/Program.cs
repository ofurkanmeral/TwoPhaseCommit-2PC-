var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var app = builder.Build();


app.MapGet("/ready", () =>
{
    var projectName = typeof(Program).Assembly.GetName().Name;
    Console.WriteLine($"{projectName} is ready");
    return true;
});

app.MapGet("/commit", () =>
{
    var projectName = typeof(Program).Assembly.GetName().Name;
    Console.WriteLine($"{projectName} is commited");
    return false;
});


app.MapGet("/rollback", () =>
{
    var projectName = typeof(Program).Assembly.GetName().Name;
    Console.WriteLine($"{projectName} is rollbacked");
});

app.Run();
