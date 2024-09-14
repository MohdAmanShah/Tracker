using System;

namespace Expense.Tracker.DataModels.ViewModels;

public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; }

}
