using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;
using Expense.Tracker.DataModels;
using Microsoft.Data.SqlClient;

namespace Expense.Tracker.DatabaseService.DataOperations;

public class CategoryOperations : DatabaseOperationsBase
{
    public CategoryOperations() : base() { }
    public CategoryOperations((DataProviderEnum provider, string connectionString) provider) : base(provider) { }
    public List<CategoryModel> GetAllCategories()
    {
        OpenConnection();
        List<CategoryModel> categories = new List<CategoryModel>();
        string sqlcommand = @"select * from tblCategory order by CategoryId";
        using (DbCommand command = _factory.CreateCommand() ?? new SqlCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    categories.Add(new CategoryModel
                    {
                        CategoryName = reader.GetString("CategoryName"),
                        CategoryId = reader.GetInt32("CategoryId")
                    });
                }
            }
        }
        return categories;
    }
    public CategoryModel GetCategory(int id)
    {
        OpenConnection();
        CategoryModel category = null;
        string sqlcommand = @$"Select * from tblCategory Where CategoryId = @id";
        using (DbCommand command = _factory.CreateCommand() ?? new SqlCommand())
        {

            DbParameter CategoryId = _factory.CreateParameter();

            CategoryId.ParameterName = "@id";
            CategoryId.Value = id;
            CategoryId.DbType = DbType.Int32;
            CategoryId.Direction = ParameterDirection.Input;

            command.Parameters.Add(CategoryId);

            command.Connection = _connection;
            command.CommandText = sqlcommand;
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    category = new CategoryModel
                    {
                        CategoryName = reader.GetString("CategoryName"),
                        CategoryId = reader.GetInt32("CategoryId")
                    };
                }
            }
        }
        return category;
    }
    public void InsertCategory(CategoryModel category)
    {
        OpenConnection();
        int rowsAffected = 0;
        string sqlcommand = @$"Insert Into tblCategory (CategoryName) Values (@name)";
        using (DbCommand command = _factory.CreateCommand() ?? new SqlCommand())
        {
            DbParameter CategoryName = _factory.CreateParameter();

            CategoryName.ParameterName = "@name";
            CategoryName.DbType = DbType.String;
            CategoryName.Direction = ParameterDirection.Input;
            CategoryName.Value = category.CategoryName;
            CategoryName.Size = 50;

            command.Parameters.Add(CategoryName);

            command.Connection = _connection;
            command.CommandText = sqlcommand;
            rowsAffected = command.ExecuteNonQuery();
        }
        CloseConnection();
    }

    public void DeleteCategory(int id)
    {
        OpenConnection();
        string sqlcommand = @$"Delete from tblCategory Where CategoryId = @id";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;
            try
            {
                DbParameter CategoryId = _factory.CreateParameter();

                CategoryId.ParameterName = "@id";
                CategoryId.Value = id;
                CategoryId.DbType = DbType.Int32;
                CategoryId.Direction = ParameterDirection.Input;

                command.Parameters.Add(CategoryId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        CloseConnection();
    }
}
