using System.Text.Json.Serialization;

namespace LearnAInalytics.Agent.YandexGPT.Models;

/// <summary>
/// Ответ ллм модели от YandexGPT
/// </summary>
internal class YandexGptResponse
{
    [JsonPropertyName("result")]
    public required Result Result { get; set; }
}
