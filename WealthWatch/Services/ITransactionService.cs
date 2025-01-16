using WealthWatch.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WealthWatch.Services
{
    public interface ITransactionService
    {
        Task<List<Transactions>> GetAllTransactionsAsync();
        Task<List<Transactions>> GetTransactionsByUserIdAsync(Guid userId);
        Task<List<Transactions>> GetTransactionsByTypeAsync(Guid userId, string type);
        Task<int> GetAmountByTypeAsync(Guid userId, string type);
        Task<bool> CreateTransactionAsync(Guid userId, Transactions transaction);
        Task<bool> UpdateTransactionAsync(Transactions transaction);
        Task<int> CheckBalance(Guid userId);
        Task<bool> CheckSufficientBalanace(int ExpenseAmount, Guid userId);
    }
}
