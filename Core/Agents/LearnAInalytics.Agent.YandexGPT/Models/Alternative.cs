using System.Text.Json.Serialization;

namespace LearnAInalytics.Agent.YandexGPT.Models;

/// <summary>
/// Альтернативы ответов ллм модели от YandexGPT
/// </summary>
internal class Alternative
{
    [JsonPropertyName("message")]
    public required Message Message { get; set; }
}
