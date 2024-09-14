using System;
using Expense.Tracker.DataModels.Models;

namespace Expense.Tracker.DataModels.ViewModels;

public class TrackerViewModelAll
{
    public required int TrackerId { get; set; }
    public required string TrackerName { get; set; }
    public required System.Boolean Flag { get; set; } = false; // Flag to mark if the Tracker is Used by single user or multiple user
    // false - single user, true - sharable, multi user.
    public required System.Boolean IsMarkedDeleted { get; set; } = false;
    public string? Link { get; set; }
    public required UserViewModel[] Owner { get; set; }
    public UserTrackerModel[]? UserTrackers { get; set; }
}
