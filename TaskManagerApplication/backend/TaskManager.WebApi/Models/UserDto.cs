namespace TaskManager.WebApi.Models;

// safe to return to clients - never includes PasswordHash
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
