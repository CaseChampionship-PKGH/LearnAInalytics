using System.IO.Compression;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Csv;
using LearnAInalytics.Parsing.Json;

namespace LearnAInalytics.Parsing.Archive;

/// <summary>
/// Парсер тестов тестируемых по архиву папки
/// </summary>
public class UserAnswersArchiveParser : IDataParser
{
    private readonly UserAnswersCsvParser csvParser;
    private readonly UserAnswersJsonParser jsonParser;
    private readonly IFormatDetector formatDetector;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserAnswersArchiveParser"/>
    /// </summary>
    public UserAnswersArchiveParser(UserAnswersCsvParser csvParser, UserAnswersJsonParser jsonParser, IFormatDetector formatDetector)
    {
        this.csvParser = csvParser;
        this.jsonParser = jsonParser;
        this.formatDetector = formatDetector;
    }

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Archive;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.UserAnswers;

    async Task<T> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(List<UserTestResult>) && typeof(T) != typeof(IEnumerable<UserTestResult>))
        {
            throw new ParsingException(
                $"Парсер папки не поддерживает тип {typeof(T).Name}. Ожидался List<UserTestResult>).");
        }

        var allResults = new List<UserTestResult>();
        using var archive = new ZipArchive(input, ZipArchiveMode.Read, leaveOpen: true);

        foreach (var entry in archive.Entries)
        {
            if (entry.Length == 0 || entry.Name.EndsWith('/'))
            {
                continue;
            }

            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            await entryStream.CopyToAsync(ms);
            ms.Position = 0;

            InputFormat format;
            try
            {
                format = formatDetector.DetectFormat(entry.Name, ms);
            }
            catch (ParsingException)
            {
                continue;
            }

            if (format == InputFormat.Archive)
            {
                continue;
            }

            List<UserTestResult>? parsed = null;

            if (format == InputFormat.Json)
            {
                parsed = await jsonParser.ParseAsync<List<UserTestResult>>(ms);
            }
            else if (format == InputFormat.Csv)
            {
                parsed = await csvParser.ParseAsync<List<UserTestResult>>(ms);
            }

            if (parsed != null)
            {
                allResults.AddRange(parsed);
            }
        }

        return (T)(object)allResults;
    }
}
