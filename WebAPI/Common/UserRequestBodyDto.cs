using System.Text.Json.Serialization;

namespace WebAPI.Common
{
    public class UserRequestBodyDto
    {
        [JsonPropertyName("login")] public string? Login { get; set; } = null;
        [JsonPropertyName("password")] public string? Password { get; set; } = null;
        [JsonPropertyName("noteId")] public Guid? NoteId { get; set; } = null;
        [JsonPropertyName("parentId")] public Guid? Parent { get; set; } = null;
        [JsonPropertyName("userId")] public Guid? User { get; set; } = null;
        [JsonPropertyName("share")] public bool? Share { get; set; } = null;
        [JsonPropertyName("check")] public bool? Check { get; set; } = null;
        [JsonPropertyName("title")] public string? Title { get; set; } = null;
        [JsonPropertyName("newPassword")] public string? NewPassword { get; set; } = null;
        [JsonPropertyName("newLogin")] public string? NewLogin { get; set; } = null;
        [JsonPropertyName("description")] public string? Description { get; set; } = null;
        [JsonPropertyName("fragment")] public string? Fragment { get; set; } = null;
        [JsonPropertyName("weight")] public double? Weight { get; set; } = null;
        [JsonPropertyName("number")] public int? Number { get; set; } = null;
    }
}
