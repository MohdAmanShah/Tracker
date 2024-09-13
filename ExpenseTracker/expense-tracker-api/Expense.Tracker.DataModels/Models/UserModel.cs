namespace Expense.Tracker.DataModels;

public class UserModel
{
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public required System.Byte[] Password { get; set; }
    public Boolean IsEmailVerified { get; set; } = false;
    public required string Email { get; set; }
    public required System.Byte[] Salt { get; set; }

}
