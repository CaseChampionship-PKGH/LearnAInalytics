using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Parsing.Csv;

/// <summary>
/// Csv парсер тестов тестируемых
/// </summary>
public class UserAnswersCsvParser : IDataParser
{
    /// <inheritdoc />
    public InputFormat Format => InputFormat.Csv;

    /// <inheritdoc />
    public ParsingTarget Target => ParsingTarget.UserAnswers;

    /// <inheritdoc />
    public async Task<T> ParseAsync<T>(Stream input)
    {
        var encoding = Encoding.GetEncoding("windows-1251");

        using var reader = new StreamReader(input, encoding, detectEncodingFromByteOrderMarks: false);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };

        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        var questionTexts = new List<string>();

        for (var i = 4; i < headers!.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(headers[i]))
            {
                questionTexts.Add(headers[i]);
            }
        }

        var results = new List<UserTestResult>();

        while (csv.Read())
        {
            var userId = csv.GetField(0);
            if (string.IsNullOrWhiteSpace(userId) || userId.StartsWith(";;;"))
            {
                continue;
            }

            var scoreString = csv.GetField(3) ?? string.Empty;
            var (score, maxScore) = ParseScore(scoreString);

            var userResult = new UserTestResult
            {
                UserId = userId,
                Date = DateTime.TryParse(csv.GetField(1), out var date) ? date : DateTime.MinValue,
                Status = csv.GetField(2) ?? string.Empty,
                Score = score,
                MaxScore = maxScore,
                Answers = []
            };

            var fieldIdx = 4;
            for (var q = 0; q < questionTexts.Count && fieldIdx + 3 < csv.Parser.Count; q++)
            {
                var correctAnswer = csv.GetField(fieldIdx + 3);
                if (string.IsNullOrWhiteSpace(correctAnswer))
                {
                    fieldIdx += 4;
                    continue;
                }

                var answer = new QuestionAnswer
                {
                    QuestionText = questionTexts[q],
                    Type = csv.GetField(fieldIdx) ?? string.Empty,
                    UserAnswer = csv.GetField(fieldIdx + 2) ?? string.Empty,
                    CorrectAnswer = correctAnswer
                };
                userResult.Answers.Add(answer);
                fieldIdx += 4;
            }

            results.Add(userResult);
        }

        return (T)(object)results;
    }

    private static (int Score, int MaxScore) ParseScore(string scoreString)
    {
        if (string.IsNullOrWhiteSpace(scoreString))
        {
            return (0, 0);
        }

        var parts = scoreString.Split('/');
        if (parts.Length < 2)
        {
            return (0, 0);
        }

        var score = 0;
        var maxScore = 0;

        _ = int.TryParse(parts[0].Trim(), out score);
        var maxPart = parts[1].Trim();
        var spaceIdx = maxPart.IndexOf(' ');
        if (spaceIdx > 0)
        {
            maxPart = maxPart.Substring(0, spaceIdx);
        }
        _ = int.TryParse(maxPart, out maxScore);

        return (score, maxScore);
    }
}
