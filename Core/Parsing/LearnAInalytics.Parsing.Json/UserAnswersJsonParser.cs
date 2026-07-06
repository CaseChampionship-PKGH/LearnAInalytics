using System.Text.Json;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Parsing.Json;

/// <summary>
/// Json парсер тестов тестируемых
/// </summary>
public class UserAnswersJsonParser : IDataParser
{
    private readonly static JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Json;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.UserAnswers;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(List<UserTestResult>))
        {
            throw new ParsingException(
                $"Парсер UserAnswersJsonParser не поддерживает тип {typeof(T).Name}. Ожидался List<UserTestResult>.");
        }

        if (input == null || input.Length == 0)
        {
            throw new ParsingException("Поток с ответами тестируемых пуст.");
        }

        string rawJson;
        using (var reader = new StreamReader(input, leaveOpen: true))
        {
            rawJson = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ParsingException("Ответы тестируемых не содержит данных.");
        }

        try
        {
            var response = JsonSerializer.Deserialize<List<UserTestResult>>(rawJson, serializerOptions);

            if (response == null || response.Count == 0)
            {
                throw new ParsingException("Десериализованные ответы тестируемых равен null или не содержит результатов.");
            }

            return (T)(object)response;
        }
        catch (JsonException ex)
        {
            throw new ParsingException($"Ошибка десериализации JSON-ответов тестируемых: {ex.Message}");
        }
    }
}
