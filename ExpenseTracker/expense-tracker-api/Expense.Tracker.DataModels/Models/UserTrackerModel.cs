using System;

namespace Expense.Tracker.DataModels.Models;

public class UserTrackerModel
{
    public required int UserId { get; set; }
    public required int TrackerId { get; set; }
    public DateTime AccessDate { get; set; }
}
