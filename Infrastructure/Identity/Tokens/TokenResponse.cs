namespace Infrastructure.Identity.Tokens;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime, Data data);

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
    public string UserName { get; set; } = default!;
    public List<string>? Roles { get; set; }
    public string CircuitName { get; set; } = default!;
    public string JamaatName { get; set; } = default!;
    public string MemberName { get; set; } = default!;
    public int CircuitId { get; set; }
    public string Email { get; set; } = default!;
    public int JamaatId { get; set; }
    public string PhoneNo { get; set; } = default!;
}


//Original Login Response
//public class Rootobject
//{
//    public Data data { get; set; }
//    public string token { get; set; }
//    public DateTime expiry { get; set; }
//    public string message { get; set; }
//    public bool status { get; set; }
//}

//public class Data
//{
//    public int userId { get; set; }
//    public string userName { get; set; }
//    public string[] roles { get; set; }
//}