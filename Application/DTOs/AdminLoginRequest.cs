namespace Application.DTOs;

public class AdminLoginRequest
{
    public string AdminUserName { get; set; } = null!;
    public string AdminPassword { get; set; } = null!;
}

public class AdminLoginResponse
{
    public int Id { get; set; }
    public string AdminUserName { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string Token { get; set; } = null!;
}
