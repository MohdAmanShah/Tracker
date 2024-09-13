using System.Data;
using System.Data.Common;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;
using Expense.Tracker.DataModels.ViewModels;
using System.Text.Json;
using System.Text;

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
}
