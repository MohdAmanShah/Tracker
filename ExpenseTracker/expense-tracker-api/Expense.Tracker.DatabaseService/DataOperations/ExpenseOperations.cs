using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;
using Expense.Tracker.DataModels.Models;

namespace Expense.Tracker.DatabaseService.DataOperations;

public class ExpenseOperations : DatabaseOperationsBase
{
    public ExpenseOperations() : base() { }
    public ExpenseOperations((DataProviderEnum provider, string connectionString) provider) : base(provider) { }

    public List<ExpenseModel> GetExpenses(int trackerid)
    {
        List<ExpenseModel> expenses = new List<ExpenseModel>();
        try
        {
            OpenConnection();
            string sqlcommand = "SELECT ExpenseId, ExpenseName, Amount, ExpenseDeleted, CategoryId, TrackerId FROM tblExpense WHERE TrackerId = @trackerid";
            using (DbCommand command = _factory.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = sqlcommand;

                command.Parameters.Add(CreateParameter("@trackerid", DbType.Int32, trackerid));

                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        expenses.Add(new ExpenseModel
                        {
                            ExpenseId = reader.GetInt32("ExpenseId"),
                            ExpenseName = reader.GetString("ExpenseName"),
                            Amount = reader.GetInt32("Amount"),
                            IsMarkedDeleted = reader.GetBoolean("ExpenseDeleted"),
                            CategoryId = reader.GetInt32("CategoryId"),
                            TrackerId = reader.GetInt32("TrackerId")
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error retrieving expenses: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
        return expenses;
    }

    public void InsertExpense(ExpenseModel expense)
    {
        try
        {
            OpenConnection();
            string sqlcommand = "INSERT INTO tblExpense(ExpenseName, Amount, ExpenseDeleted, CategoryId, TrackerId) VALUES (@expensename, @amount, @expensedeleted, @categoryid, @trackerid)";
            using (DbCommand command = _factory.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = sqlcommand;

                command.Parameters.Add(CreateParameter("@expensename", DbType.String, expense.ExpenseName));
                command.Parameters.Add(CreateParameter("@amount", DbType.Int32, expense.Amount));
                command.Parameters.Add(CreateParameter("@expensedeleted", DbType.Boolean, expense.IsMarkedDeleted));
                command.Parameters.Add(CreateParameter("@categoryid", DbType.Int32, expense.CategoryId));
                command.Parameters.Add(CreateParameter("@trackerid", DbType.Int32, expense.TrackerId));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error inserting expense: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    public void MarkAsDeleted(int expenseid)
    {
        try
        {
            OpenConnection();
            string sqlcommand = "UPDATE tblExpense SET ExpenseDeleted = 1 WHERE ExpenseId = @id";
            using (DbCommand command = _factory.CreateCommand())
            {
                command.CommandText = sqlcommand;
                command.Connection = _connection;

                command.Parameters.Add(CreateParameter("@id", DbType.Int32, expenseid));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error marking expense as deleted: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    public void RecoverDeleted(int expenseid)
    {
        try
        {
            OpenConnection();
            string sqlcommand = "UPDATE tblExpense SET ExpenseDeleted = 0 WHERE ExpenseId = @id";
            using (DbCommand command = _factory.CreateCommand())
            {
                command.CommandText = sqlcommand;
                command.Connection = _connection;

                command.Parameters.Add(CreateParameter("@id", DbType.Int32, expenseid));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error recovering expense: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    private DbParameter CreateParameter(string name, DbType type, object value, ParameterDirection direction = ParameterDirection.Input)
    {
        DbParameter param = _factory.CreateParameter();
        param.ParameterName = name;
        param.DbType = type;
        param.Value = value;
        param.Direction = direction;
        return param;
    }
}
