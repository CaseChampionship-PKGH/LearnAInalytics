using System.Text.Json;
using LearnAInalytics.Agent.Contracts.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Parsing.Json;

/// <summary>
/// <inheritdoc cref="IDataParser"/> для ответов агента
/// </summary>
public class AgentResponseJsonParser : IDataParser
{
    private readonly static JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Json;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.AgentResponse;

    async Task<T> IDataParser.ParseAsync<T>(Stream input, string fileName)
    {
        if (typeof(T) != typeof(AgentBatchResponse))
        {
            throw new ParsingException(
                $"Парсер AgentResponse не поддерживает тип {typeof(T).Name}. Ожидался AgentBatchResponse.");
        }

        if (input == null || input.Length == 0)
        {
            throw new ParsingException("Поток с ответом агента пуст.");
        }

        string rawJson;
        using (var reader = new StreamReader(input, leaveOpen: true))
        {
            rawJson = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ParsingException("Ответ агента не содержит данных.");
        }

        try
        {
            var response = JsonSerializer.Deserialize<AgentBatchResponse>(rawJson, serializerOptions);

            if (response == null || response.Results == null)
            {
                throw new ParsingException("Десериализованный ответ агента равен null или не содержит результатов.");
            }

            return (T)(object)response;
        }
        catch (JsonException ex)
        {
            throw new ParsingException($"Ошибка десериализации JSON-ответа агента: {ex.Message}");
        }
    }
}
