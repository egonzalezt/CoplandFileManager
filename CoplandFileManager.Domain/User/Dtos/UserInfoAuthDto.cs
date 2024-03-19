namespace CoplandFileManager.Domain.User.Dtos;

using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

public class UserInfoAuthDto
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }

    public static UserInfoAuthDto? DecodeUserInfoAuth(string base64String)
    {
        var decodedString = Decode64(base64String);
        return JsonSerializer.Deserialize<UserInfoAuthDto>(decodedString);
    }

    private static string Decode64(string text)
    {
        text = text.Replace('_', '/').Replace('-', '+');
        switch (text.Length % 4)
        {
            case 2:
                text += "==";
                break;
            case 3:
                text += "=";
                break;
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(text));
    }

}
