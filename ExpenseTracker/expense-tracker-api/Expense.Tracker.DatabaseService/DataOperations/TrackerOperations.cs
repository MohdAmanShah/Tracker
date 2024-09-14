using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;
using Expense.Tracker.DataModels.ViewModels;
using System.Text.Json;
using System.Text;
using Expense.Tracker.DataModels;

namespace Expense.Tracker.DatabaseService.DataOperations;

public class TrackerOperations : DatabaseOperationsBase
{
    public TrackerOperations() : base() { }
    public TrackerOperations((DataProviderEnum provider, string connectionString) provider) : base(provider) { }
    public TrackerViewModelAll GetTracker(int id)
    {
        TrackerViewModelAll[] tracker = null;
        OpenConnection();
        string sqlcommand = @"SELECT DISTINCT
        t.TrackerId,
        t.TrackerName,
        t.Flag,
        t.TrackerDeleted AS IsMarkedDeleted,
        t.Link,
        (SELECT TOP 1
            UserId,
            UserName,
            Email,
            EmailVerified AS IsEmailVerified
        FROM tblUser
        WHERE UserId = t.OwnerId
        FOR JSON PATH) AS Owner,
        (SELECT
            ut.UserId,
            ut.TrackerId,
            ut.AccessDate
        FROM tblUserTracker ut
        WHERE ut.TrackerId = t.TrackerId
        FOR JSON PATH) AS UserTrackers
    FROM tblTracker t
        INNER JOIN tblUser u ON t.OwnerId = u.UserId
        LEFT JOIN tblUserTracker ut ON t.TrackerId = ut.TrackerId
    WHERE t.TrackerId = @TrackerId
    FOR JSON PATH;";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.CommandText = sqlcommand;
            command.Connection = _connection;

            DbParameter param = _factory.CreateParameter();
            param.ParameterName = "@TrackerId";
            param.Value = id;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Int32;

            command.Parameters.Add(param);

            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    string jsonData = reader.GetString(0);
                    tracker = JsonSerializer.Deserialize<TrackerViewModelAll[]>(jsonData);
                }
            }

        }
        return tracker[0];
    }
    public List<TrackerViewModelAll> GetAllTrackers()
    {
        List<TrackerViewModelAll> trackers = null;
        StringBuilder result = new StringBuilder();
        OpenConnection();
        string sqlcommand = @"SELECT DISTINCT
        t.TrackerId,
        t.TrackerName,
        t.Flag,
        t.TrackerDeleted AS IsMarkedDeleted,
        t.Link,
        (SELECT TOP 1
            UserId,
            UserName,
            Email,
            EmailVerified AS IsEmailVerified
        FROM tblUser
        WHERE UserId = t.OwnerId
        FOR JSON PATH) AS Owner,
        (SELECT
            ut.UserId,
            ut.TrackerId,
            ut.AccessDate
        FROM tblUserTracker ut
        WHERE ut.TrackerId = t.TrackerId
        FOR JSON PATH) UserTrackers
    FROM tblTracker t
        INNER JOIN tblUser u ON t.OwnerId = u.UserId
        LEFT JOIN tblUserTracker ut ON t.TrackerId = ut.TrackerId
    FOR JSON PATH;";
        using (DbCommand command = _factory.CreateCommand())
        {
            command.CommandText = sqlcommand;
            command.Connection = _connection;
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    result.Append(reader.GetString(0));
                }
                trackers = JsonSerializer.Deserialize<List<TrackerViewModelAll>>(result.ToString());
            }
        }
        return trackers;
    }
    public int InsertTracker(TrackerModel tracker)
    {
        int id = 0;
        try
        {
            string sqlcommand = @"INSERT INTO tblTracker(TrackerName, Flag, Link, OwnerId) OUTPUT INSERTED.TrackerId VALUES(@trackername, @flag, @link, @ownerid);";
            OpenConnection();
            using (DbCommand command = _factory.CreateCommand())
            {
                command.CommandText = sqlcommand;
                command.Connection = _connection;

                DbParameter param = _factory.CreateParameter();
                param.ParameterName = "@trackername";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.TrackerName;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@flag";
                param.DbType = DbType.Boolean;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.Flag;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@link";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.Link;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@ownerid";
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                param.Value = tracker.OwnerId;
                command.Parameters.Add(param);

                id = (int)command.ExecuteScalar();
            }
        }
        catch (Exception ex) { }
        finally
        {
            CloseConnection();
        }
        return id;
    }
    public void MarkAsDeleted(int id)
    {
        try
        {
            string sqlcommand = "UPDATE tblTracker SET TrackerDeleted = 1 WHERE TrackerId = @trackerid";
            OpenConnection();
            using (DbCommand command = _factory.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = sqlcommand;

                DbParameter param = _factory.CreateParameter();
                param.ParameterName = "@trackerid";
                param.DbType = DbType.Int32;
                param.Direction = ParameterDirection.Input;
                param.Value = id;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex) { }
        finally
        {
            CloseConnection();
        }
    }
    public void RecoverTracker(int id)
    {
        try
        {
            string sqlcommand = "UPDATE tblTracker SET TrackerDeleted = 0 WHERE TrackerId = @trackerid";
            OpenConnection();
            using (DbCommand command = _factory.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = sqlcommand;

                DbParameter param = _factory.CreateParameter();
                param.ParameterName = "@trackerid";
                param.DbType = DbType.Int32;
                param.Direction = ParameterDirection.Input;
                param.Value = id;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        { }
        finally
        {
            CloseConnection();
        }
    }
    public void UpdateTracker(TrackerModel tracker)
    {
        try
        {
            string sqlcommand = "UPDATE tblTracker SET TrackerName = @trackername, Flag = @flag, Link = @link WHERE TrackerId = @trackerid";
            OpenConnection();
            using (DbCommand command = _factory.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandText = sqlcommand;

                DbParameter param = _factory.CreateParameter();
                param.ParameterName = "@trackername";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.TrackerName;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@flag";
                param.DbType = DbType.Boolean;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.Flag;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@link";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.Link;
                command.Parameters.Add(param);

                param = _factory.CreateParameter();
                param.ParameterName = "@trackerid";
                param.DbType = DbType.Int32;
                param.Direction = ParameterDirection.Input;
                param.Value = tracker.TrackerId;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex) { }
        finally
        {
            CloseConnection();
        }
    }
}
