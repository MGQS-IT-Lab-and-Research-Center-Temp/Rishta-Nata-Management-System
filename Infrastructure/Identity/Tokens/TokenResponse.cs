namespace Infrastructure.Identity.Tokens;

public class MemberApiLoginResponse
{
    public Data Data { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime Expiry { get; set; }
    public string Message { get; set; } = default!;
    public bool Status { get; set; }
}

public class Data
{
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string[] Roles { get; set; } = [];
}