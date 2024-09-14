using System.Buffers;
using Expense.Tracker.DatabaseService.DataOperations;
using Expense.Tracker.DataModels;
using Expense.Tracker.DataModels.ViewModels;

Console.Clear();
PrintTrackerList();


void PrintCategoryList()
{
    using (CategoryOperations operations = new CategoryOperations())
    {
        var categories = operations.GetAllCategories();
        foreach (var category in categories)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(category.CategoryId);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(": {0}", category.CategoryName);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}


void InsertIntoCategory(string name)
{
    using (CategoryOperations operations = new CategoryOperations())
    {
        CategoryModel category = new CategoryModel
        {
            CategoryName = name,
        };
        operations.InsertCategory(category);
    }
}

void DeleteFromCategory(int id)
{
    using (CategoryOperations operations = new CategoryOperations())
    {
        operations.DeleteCategory(id);
    }
}

void PrintTracker(int id)
{
    using (TrackerOperations operations = new TrackerOperations())
    {
        TrackerViewModelAll tracker = operations.GetTracker(id);
        Console.WriteLine(tracker.TrackerName);
        Console.WriteLine(tracker.Owner[0].UserName);
        Console.WriteLine("Number of users: {0}", tracker.UserTrackers.Length + 1);
    }
}

void PrintTrackerList()
{
    using (TrackerOperations operations = new TrackerOperations())
    {
        foreach (TrackerViewModelAll tracker in operations.GetAllTrackers())
        {
            Console.WriteLine(tracker.TrackerName);
            Console.WriteLine(tracker.Owner[0].UserName);
            Console.WriteLine("Number of users: {0}", tracker.UserTrackers?.Length + 1);
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
        }
    }
}