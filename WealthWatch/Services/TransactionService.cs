using System.Collections.Generic;
using System.Text.Json;
using WealthWatch.Models;
using Windows.System;

namespace WealthWatch.Services
{
    public class TransactionService
    {
        private readonly string _dataFilePath;

        public TransactionService()
        {
            // Set the file path to the desired directory for transactions
            string appDataPath = Path.Combine("C:\\sandeep");
            
            _dataFilePath = Path.Combine(appDataPath, "transactions.json");

            // Ensure the directory exists
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
        }

        public async Task SaveTransactionsAsync(Guid userId, List<Transactions> userTransactions)
        {
            try
            {
                // Retrieve all existing transactions
                var allTransactions = await GetAllTransactionsAsync();

                // Remove any existing transactions for the given UserId
                allTransactions.RemoveAll(t => t.UserId == userId);

                // Add the updated transactions for the given UserId
                allTransactions.AddRange(userTransactions);

                // Save all transactions back to the file
                string json = JsonSerializer.Serialize(allTransactions);
                await File.WriteAllTextAsync(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }

        public async Task<List<Transactions>> GetTransactionsByUserIdAsync(Guid userId)
        {
            try
            {
                // Get all transactions
                var allTransactions = await GetAllTransactionsAsync();

                // Filter transactions for the specific user
                return allTransactions.Where(t => t.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving transactions for UserId {userId}: {ex.Message}");
                return new List<Transactions>();
            }
        }
        public async Task<List<Transactions>> GetTransactionsByTypeAsync(Guid userId, string type)
        {
            // Get all transactions for the user
            var allTransactions = await GetTransactionsByUserIdAsync(userId);

            // Filter transactions by the given type
            var transactionByType = allTransactions.Where(t => t.Type == type).ToList();

            return transactionByType;
        }

        public async Task<int> GetAmountByTypeAsync(Guid userId, string type)
        {
            // Get all transactions of the specified type
            var transactionsByType = await GetTransactionsByTypeAsync(userId, type);

            // Calculate the total amount by summing all valid integer amounts
            int totalAmount = transactionsByType
                .Sum(t => int.TryParse(t.Amount, out var amount) ? amount : 0);


            return totalAmount;
        }

        public async Task<List<Transactions>> GetAllTransactionsAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    return new List<Transactions>();
                }

                string jsonTransactions = await File.ReadAllTextAsync(_dataFilePath);
                return JsonSerializer.Deserialize<List<Transactions>>(jsonTransactions) ?? new List<Transactions>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing transactions.json: {ex.Message}");
                return new List<Transactions>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Transactions>();
            }
        }



        public async Task<bool> CreateTransactionAsync(Guid userId, Transactions transaction)
        {
            var transactions = await GetAllTransactionsAsync();

            // Associate the transaction with the user ID
            transaction.UserId = userId;

            // Add the transaction to the list and save it
            transactions.Add(transaction);
            await SaveTransactionsAsync(userId, transactions);
            return true;
        }


       
        public async Task<List<Transactions>> GetAllUserTransactionsAsync()
        {
            return await GetAllTransactionsAsync();
        }
        public async Task<bool> UpdateTransactionAsync(Transactions transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

                // Retrieve all existing transactions
                var allTransactions = await GetAllTransactionsAsync();

                // Find the transaction to update
                var existingTransaction = allTransactions.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);

                if (existingTransaction == null)
                {
                    Console.WriteLine($"Transaction with ID {transaction.TransactionId} not found.");
                    return false; // Transaction not found
                }

                // Update the properties of the existing transaction
                existingTransaction.Source = transaction.Source;
                existingTransaction.Date = transaction.Date;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.DueDate = transaction.DueDate;
                existingTransaction.Status = transaction.Status;
                existingTransaction.Type = transaction.Type;

                // Save the updated transactions back to the file
                await SaveTransactionsAsync(existingTransaction.UserId, allTransactions);

                return true; // Update successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating transaction: {ex.Message}");
                return false; // Update failed
            }
        }

        public async Task<int> CheckBalance(Guid userId)
        {
            int IncomeAmount = await GetAmountByTypeAsync(userId, "income");
            int DebtAmount = await GetAmountByTypeAsync(userId, "debt");
            int ExpenseAmount = await GetAmountByTypeAsync(userId, "expense");

            int Balance = (IncomeAmount + DebtAmount) - ExpenseAmount;
            return Balance;
        }

        public async Task<bool> CheckSufficientBalanace(int ExpenseAmount, Guid userId)
        {
            int Balance = await CheckBalance(userId);
            if (Balance > ExpenseAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
