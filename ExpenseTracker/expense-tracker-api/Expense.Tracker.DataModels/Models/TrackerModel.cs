using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Tracker.DataModels;

public class TrackerModel
{
    public required string TrackerName { get; set; }
    public required System.Boolean Flag { get; set; } = false; // Flag to mark if the Tracker is Used by single user or multiple user
    // false - single user, true - sharable, multi user.
    public required System.Boolean IsMarkedDeleted { get; set; } = false;
    public string? Link { get; set; }
    public required int OwnerId { get; set; }
    public required int TrackerId { get; set; }
}
