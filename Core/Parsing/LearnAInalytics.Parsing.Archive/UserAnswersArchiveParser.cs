using System.IO.Compression;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Csv;
using LearnAInalytics.Parsing.Excel;

namespace LearnAInalytics.Parsing.Archive;

/// <summary>
/// Парсер ответов тестируемых по архиву папки
/// </summary>
public class UserAnswersArchiveParser : IDataParser
{
    private readonly SurveyCsvParser csvParser;
    private readonly SurveyExcelParser excelParser;
    private readonly IFormatDetector formatDetector;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserAnswersArchiveParser"/>
    /// </summary>
    public UserAnswersArchiveParser(SurveyCsvParser csvParser, SurveyExcelParser excelParser, IFormatDetector formatDetector)
    {
        this.csvParser = csvParser;
        this.excelParser = excelParser;
        this.formatDetector = formatDetector;
    }

    /// <inheritdoc />
    public InputFormat Format => InputFormat.Archive;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.Survey;

    async Task<T> IDataParser.ParseAsync<T>(Stream input)
    {
        if (typeof(T) != typeof(List<SurveyResponse>) && typeof(T) != typeof(IEnumerable<SurveyResponse>))
        {
            throw new ParsingException(
                $"Парсер папки не поддерживает тип {typeof(T).Name}. Ожидался List<SurveyResponse>).");
        }

        var allResults = new List<SurveyResponse>();
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

            List<SurveyResponse>? parsed = null;

            if (format == InputFormat.Excel)
            {
                parsed = await excelParser.ParseAsync<List<SurveyResponse>>(ms);
            }
            else if (format == InputFormat.Csv)
            {
                parsed = await csvParser.ParseAsync<List<SurveyResponse>>(ms);
            }

            if (parsed != null)
            {
                allResults.AddRange(parsed);
            }
        }

        return (T)(object)allResults;
    }
}
