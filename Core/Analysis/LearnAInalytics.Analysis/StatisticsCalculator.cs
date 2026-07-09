using LearnAInalytics.Analysis.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Entities.Enums;
using LearnAInalytics.Parsing.Contracts.Models;

namespace LearnAInalytics.Analysis;

/// <inheritdoc cref="IStatisticsCalculator"/>
public class StatisticsCalculator : IStatisticsCalculator
{
    SurveyStatistics IStatisticsCalculator.Calculate(SurveyParseResult parsedData)
    {
        var stats = new SurveyStatistics
        {
            TotalRespondents = parsedData.Responses.Select(r => r.RespondentId).Distinct().Count()
        };

        var answersByQuestion = parsedData.Responses
            .SelectMany(r => r.Answers)
            .GroupBy(a => a.QuestionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var overallSum = 0.0;
        var overallCount = 0;

        foreach (var question in parsedData.Questions)
        {
            if (!answersByQuestion.TryGetValue(question.QuestionId, out var answers) || answers.Count == 0)
            {
                continue;
            }

            if (question.Type == QuestionType.OpenText)
            {
                continue;
            }

            var qStats = new QuestionStatistics
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                Type = question.Type
            };

            switch (question.Type)
            {
                case QuestionType.Numeric:
                    var numericValues = answers
                        .Where(a => a.NumericValue.HasValue)
                        .Select(a => a.NumericValue!.Value)
                        .ToList();

                    if (numericValues.Any())
                    {
                        qStats.Average = numericValues.Average();
                        qStats.Median = CalculateMedian(numericValues);
                        qStats.StandardDeviation = CalculateStdDev(numericValues, qStats.Average.Value);
                        qStats.Distribution = numericValues
                            .GroupBy(v => (int)Math.Round(v))
                            .ToDictionary(g => g.Key, g => g.Count());

                        var total = numericValues.Count;
                        qStats.PercentLow = 100.0 * numericValues.Count(v => v >= 1 && v <= 3) / total;
                        qStats.PercentMedium = 100.0 * numericValues.Count(v => v >= 4 && v <= 7) / total;
                        qStats.PercentHigh = 100.0 * numericValues.Count(v => v >= 8 && v <= 10) / total;

                        overallSum += numericValues.Sum();
                        overallCount += total;
                    }
                    break;

                case QuestionType.Binary:
                    var yes = answers.Count(a => a.BinaryValue?.ToLower() == "да");
                    var no = answers.Count(a => a.BinaryValue?.ToLower() == "нет");
                    qStats.YesCount = yes;
                    qStats.NoCount = no;
                    var binaryTotal = yes + no;
                    if (binaryTotal > 0)
                    {
                        qStats.YesPercent = 100.0 * yes / binaryTotal;
                        qStats.NoPercent = 100.0 * no / binaryTotal;
                    }
                    break;

                default:
                    break;
            }

            stats.Questions.Add(qStats);
        }

        if (overallCount > 0)
        {
            stats.OverallNumericAverage = overallSum / overallCount;
        }

        return stats;
    }

    private static double CalculateMedian(List<double> values)
    {
        var sorted = values.OrderBy(v => v).ToList();
        var count = sorted.Count;
        if (count == 0)
        {
            return 0;
        }

        return count % 2 == 1 ? sorted[count / 2] : (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
    }

    private static double CalculateStdDev(List<double> values, double mean)
    {
        var sumSq = values.Sum(v => Math.Pow(v - mean, 2));
        return Math.Sqrt(sumSq / values.Count);
    }
}
