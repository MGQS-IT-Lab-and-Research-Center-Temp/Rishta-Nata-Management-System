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
    public int userId { get; set; }
    public string userName { get; set; } = default!;
    public string[] roles { get; set; } = [];
}