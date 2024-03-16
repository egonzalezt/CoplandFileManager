using System.Text.Json.Serialization;

namespace CoplandFileManager.Domain.User.Dtos;

public class UserInfoAuthDto
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
