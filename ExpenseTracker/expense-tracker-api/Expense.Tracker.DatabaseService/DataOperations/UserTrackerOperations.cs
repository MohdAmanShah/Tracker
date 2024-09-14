using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;

namespace Expense.Tracker.DatabaseService.DataOperations;

public class UserTrackerOperations : DatabaseOperationsBase
{
    public UserTrackerOperations() : base() { }
    public UserTrackerOperations((DataProviderEnum provider, string connectionString) provider) : base(provider) { }
    public void InsertUserTracker(int userid, int trackerid)
    {
        try
        {
            string sqlcommand = "INSERT INTO tblUserTracker(UserId, TrackerId) VALUES(@userid, @trackerid)";
            OpenConnection();
            using (DbCommand command = _factory.CreateCommand())
            {
                command.CommandText = sqlcommand;
                command.Connection = _connection;

                command.Parameters.Add(CreateParameter("@userid", DbType.Int32, userid));
                command.Parameters.Add(CreateParameter("@trackerid", DbType.Int32, trackerid));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex) { }
        finally { CloseConnection(); }
    }
}