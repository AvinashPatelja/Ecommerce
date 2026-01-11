namespace IdentityService.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsApproved { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedOn { get; set; }
}
