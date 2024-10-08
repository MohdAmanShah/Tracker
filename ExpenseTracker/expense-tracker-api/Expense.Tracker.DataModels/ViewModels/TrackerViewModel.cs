using System;

namespace Expense.Tracker.DataModels.ViewModels;

public class TrackerViewModel
{
    public required string TrackerName { get; set; }
    public required System.Boolean Flag { get; set; } = false; // Flag to mark if the Tracker is Used by single user or multiple user
    // false - single user, true - sharable, multi user.
    public string? Link { get; set; }
    public required int OwnerId { get; set; }
    public int TrackerId { get; set; }

    public DateTime CreationDate { get; set; }
}
