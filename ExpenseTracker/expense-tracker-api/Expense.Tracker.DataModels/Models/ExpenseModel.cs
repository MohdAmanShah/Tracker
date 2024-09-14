using System;

namespace Expense.Tracker.DataModels.Models;

public class ExpenseModel
{
    public required int ExpenseId { get; set; }
    public required string ExpenseName { get; set; }
    public required int Amount { get; set; }
    public System.Boolean IsMarkedDeleted { get; set; } = false;
    public required int CategoryId { get; set; }
    public required int TrackerId { get; set; }
}
