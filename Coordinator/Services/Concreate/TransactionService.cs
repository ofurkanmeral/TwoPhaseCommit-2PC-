using Coordinator.Context;
using Coordinator.Models;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Coordinator.Models.Enums;

namespace Coordinator.Services.Concreate
{
    public class TransactionService(TwoPhaseCommitContext _context,IHttpClientFactory _httpClientFactory) : ITransactionService
    {

        HttpClient _orderHttpClient = _httpClientFactory.CreateClient("OrderAPI");
        HttpClient _stockHttpClient = _httpClientFactory.CreateClient("StockAPI");
        HttpClient _paymentHttpClient = _httpClientFactory.CreateClient("PaymentAPI");    

        public async Task<Guid> CreateTransactionAsync()
        {
            Guid transactionId = Guid.NewGuid();
            var nodes = await _context.Nodes.ToListAsync();
            nodes.ForEach(node => node.NodeStates = new List<NodeState>() 
            { 
                new(transactionId) 
                { IsReady = ReadyType.Pending, TransactionState = TransactionState.Pending } 
            });
            await _context.SaveChangesAsync();  
            return transactionId;
        }
        public async Task PrepareServiceAsync(Guid transactionId)
        {
            var transactionNodes=await _context.NodeStates
                .Include(x=>x.Node)
                .Where(x=>x.TransactionId == transactionId)
                    .ToListAsync();
            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.name switch
                    {
                        "OrderAPI" => _orderHttpClient.GetAsync("ready"),
                        "StockAPI" => _stockHttpClient.GetAsync("ready"),
                        "PaymentAPI" => _paymentHttpClient.GetAsync("ready"),
                    });
                    var result=bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.IsReady = result ? ReadyType.Ready : ReadyType.UnReady;
                }
                catch (Exception)
                {
                    transactionNode.IsReady=ReadyType.UnReady;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckReadyServiceAsync(Guid transactionId)
            =>(await _context.NodeStates.Where(x=>x.TransactionId==transactionId).ToListAsync()).TrueForAll(x=>x.IsReady==ReadyType.Ready);

        public async Task CommitAsync(Guid transactionId)
        {
            var transactionNodes=await _context.NodeStates
                .Where(x=>x.TransactionId==transactionId)
                .Include(x=>x.Node)
                .ToListAsync();

            foreach (var transationNode in transactionNodes)
            {
                try
                {
                    var response = await (transationNode.Node.name switch
                    {
                        "OrderAPI" => _orderHttpClient.GetAsync("commit"),
                        "StockAPI" => _stockHttpClient.GetAsync("commit"),
                        "PaymentAPI" => _paymentHttpClient.GetAsync("commit")
                    });
                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    transationNode.TransactionState = result ? TransactionState.Success : TransactionState.Abort;
                }
                catch 
                {
                    transationNode.TransactionState=TransactionState.Abort;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckTransactionStateServicesAsync(Guid transactionId)
            => (await _context.NodeStates.Where(x => x.TransactionId == transactionId).ToListAsync()).TrueForAll(x => x.TransactionState == TransactionState.Success);
      
        public async Task RoolbackAsync(Guid transactionId)
        {
            var transactionNodes=await _context.NodeStates
                .Where(x=>x.TransactionId == transactionId)
                .Include(x=>x.Node)
                .ToListAsync();
            foreach(var transactionNode in transactionNodes)
            {
                try
                {
                    if(transactionNode.TransactionState == TransactionState.Success)
                        _ = await (transactionNode.Node.name switch
                        {
                            "OrderAPI" => _orderHttpClient.GetAsync("rollback"),
                            "StockAPI" => _stockHttpClient.GetAsync("rollback"),
                            "PaymentAPI" => _paymentHttpClient.GetAsync("rollback")
                        });
                    transactionNode.TransactionState = TransactionState.Abort;
                }
                catch 
                {
                    transactionNode.TransactionState = TransactionState.Abort;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
