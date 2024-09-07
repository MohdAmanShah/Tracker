namespace Expense.Tracker.DataModels;

public class Class1
{
    public int UserId { get; set; }
    public required string Name { get; set; }

    public required string Password { get; set; }

    public Boolean IsEmailVerified { get; set; }

    public required string Email { get; set; }

    public required string Salt { get; set; }

}
