using Coordinator.Context;
using Coordinator.Services.Abstractions;
using Coordinator.Services.Concreate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TwoPhaseCommitContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress = new("https://localhost:7225"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7169"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7032"));

builder.Services.AddTransient<ITransactionService, TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("create-order-transaction", async (ITransactionService transactionService) =>
{
    var transactionId = await transactionService.CreateTransactionAsync();
    await transactionService.PrepareServiceAsync(transactionId);
    bool transactionState = await transactionService.CheckReadyServiceAsync(transactionId);
    if (transactionState)
    {
        await transactionService.CommitAsync(transactionId);
        transactionState = await transactionService.CheckTransactionStateServicesAsync(transactionId);
    }
    if (!transactionState)
        await transactionService.RoolbackAsync(transactionId);
});

app.Run();
