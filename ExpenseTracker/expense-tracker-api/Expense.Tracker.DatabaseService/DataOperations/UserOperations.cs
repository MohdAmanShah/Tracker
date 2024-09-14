using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;
using Expense.Tracker.DataModels;
using Expense.Tracker.DataModels.ViewModels;

namespace Expense.Tracker.DatabaseService.DataOperations;

public class UserOperations : DatabaseOperationsBase
{
    public UserOperations() : base() { }
    public UserOperations((DataProviderEnum provider, string connectionString) provider) : base(provider) { }
    public List<UserViewModel> GetAllUsers()
    {
        List<UserViewModel> users = new List<UserViewModel>();
        OpenConnection();
        string sqlcommand = "SELECT UserId, UserName, Email, EmailVerified FROM tblUser;";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;

            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    users.Add(new UserViewModel
                    {
                        UserName = reader.GetString("UserName"),
                        UserId = reader.GetInt32("UserId"),
                        Email = reader.GetString("Email"),
                        IsEmailVerified = reader.GetBoolean("EmailVerified")
                    });
                }
            }
        }
        return users;
    }
    public UserViewModel GetUser(int id)
    {
        UserViewModel user = null;
        OpenConnection();
        string sqlcommand = "SELECT UserId, UserName, Email, EmailVerified FROM tblUser WHERE UserId = @id";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;

            DbParameter userId = _factory.CreateParameter();
            userId.ParameterName = "@id";
            userId.Value = id;
            userId.Direction = ParameterDirection.Input;
            userId.DbType = DbType.Int32;

            command.Parameters.Add(userId);

            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    user = new UserViewModel
                    {
                        UserName = reader.GetString("UserName"),
                        UserId = reader.GetInt32("UserId"),
                        Email = reader.GetString("Email"),
                        IsEmailVerified = reader.GetBoolean("EmailVerified")
                    };
                }
            }
        }
        return user;
    }
    public void InsertUser(UserModel user)
    {
        OpenConnection();
        string sqlcommand = "INSERT INTO tblUser (UserName, Email, [Password], Salt, EmailVerified) VALUES (@username,@email,@password,@salt,@emailverified)";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;

            DbParameter param = _factory.CreateParameter();
            param.ParameterName = "@username";
            param.Value = user.UserName;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.String;
            param.Size = 40;
            command.Parameters.Add(param);

            param = _factory.CreateParameter();
            param.ParameterName = "@email";
            param.Value = user.Email;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.String;
            param.Size = 50;
            command.Parameters.Add(param);

            param = _factory.CreateParameter();
            param.ParameterName = "@password";
            param.Value = user.Password;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Binary;
            param.Size = 60;
            command.Parameters.Add(param);

            param = _factory.CreateParameter();
            param.ParameterName = "@salt";
            param.Value = user.Salt;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Binary;
            param.Size = 60;
            command.Parameters.Add(param);

            param = _factory.CreateParameter();
            param.ParameterName = "@emailverified";
            param.Value = user.IsEmailVerified;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Boolean;
            command.Parameters.Add(param);

            command.ExecuteNonQuery();
        }
        CloseConnection();
    }

    public void DeleteUser(int id)
    {
        OpenConnection();
        string sqlcommand = "DELETE FROM tblUser WHERE UserId = @id";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;

            DbParameter param = _factory.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Int32;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
        CloseConnection();
    }
    public void UpdateUser(int id, byte[] password)
    {
        OpenConnection();
        string sqlcommand = "UPDATE tblUser SET Password = @password WHERE UserId = @id";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.Connection = _connection;
            command.CommandText = sqlcommand;

            DbParameter param = _factory.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Int32;

            command.Parameters.Add(param);

            param = _factory.CreateParameter();
            param.ParameterName = "@password";
            param.Value = password;
            param.DbType = DbType.Binary;
            param.Direction = ParameterDirection.Input;
            param.Size = 60;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
        CloseConnection();
    }
}
