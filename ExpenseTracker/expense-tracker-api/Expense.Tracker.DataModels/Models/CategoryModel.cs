using System;

namespace Expense.Tracker.DataModels;

public class CategoryModel
{
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
}
