using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Enums;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Reporting.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Services.Contracts.Constants;

namespace LearnAInalytics.Reporting;

/// <inheritdoc cref="IReportBuilder"/>
public class AgentReportBuilder : IReportBuilder
{
    private readonly IReportAgent reportAgent;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AgentReportBuilder"/>
    /// </summary>
    public AgentReportBuilder(IReportAgent reportAgent)
    {
        this.reportAgent = reportAgent;
    }

    async Task<ReportData> IReportBuilder.BuildAsync(List<QuestionAnalysisResult> questionResults, AnalysisMethod analysisMethod)
    {
        var questionReports = new List<QuestionReport>();
        var allUserIds = new HashSet<string>();

        foreach (var qr in questionResults)
        {
            var correct = qr.Results.Count(r => r.Verdict == "correct");
            var partial = qr.Results.Count(r => r.Verdict == "partial");
            var incorrect = qr.Results.Count(r => r.Verdict == "incorrect");
            var total = qr.Results.Count;
            var correctPct = total > 0 ? (double)correct / total * 100.0 : 0;
            var avgSimilarity = total > 0 ? qr.Results.Average(r => r.SimilarityPercent) : 0;

            var commonMistakes = qr.Results
                .Where(r => r.Verdict == "incorrect" && !string.IsNullOrWhiteSpace(r.Comment))
                .GroupBy(r => r.Comment)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => $"{g.Key} ({g.Count()} раз(а))")
                .ToList();

            foreach (var r in qr.Results)
            {
                allUserIds.Add(r.UserId);
            }

            questionReports.Add(new QuestionReport
            {
                QuestionText = qr.Question.QuestionText,
                CorrectAnswer = qr.Question.CorrectAnswer,
                Type = qr.Question.Type,
                TotalAnswers = total,
                CorrectCount = correct,
                PartialCount = partial,
                IncorrectCount = incorrect,
                CorrectPercentage = Math.Round(correctPct, 1),
                IsCritical = correctPct < TestAnalysisConstants.CriticalThreshold,
                CommonMistakes = commonMistakes,
                EmptyCount = qr.Results.Count(r => r.Verdict == "incorrect" && r.SimilarityPercent == 0 && r.Comment?.Contains("пустой") == true),
                AverageSimilarityPercent = Math.Round(avgSimilarity, 1)
            });
        }

        var criticalIssues = questionReports
            .Where(q => q.IsCritical)
            .Select(q => $"Вопрос: \"{Truncate(q.QuestionText, 80)}\" – правильных ответов: {q.CorrectPercentage}%")
            .ToList();

        var totalQuestions = questionReports.Count;
        var totalUsers = allUserIds.Count;
        var overallCorrect = questionReports.Any() ? questionReports.Average(q => q.CorrectPercentage) : 0;
        var overallPartial = questionReports.Any() ? questionReports.Average(q => (double)q.PartialCount / q.TotalAnswers * 100) : 0;
        var overallIncorrect = questionReports.Any() ? questionReports.Average(q => (double)q.IncorrectCount / q.TotalAnswers * 100) : 0;

        var summary = new Summary
        {
            TotalQuestions = totalQuestions,
            TotalUsers = totalUsers,
            OverallCorrectPercentage = Math.Round(overallCorrect, 1),
            OverallPartialPercentage = Math.Round(overallPartial, 1),
            OverallIncorrectPercentage = Math.Round(overallIncorrect, 1)
        };

        var reportData = await reportAgent.GenerateReportAsync(summary, criticalIssues, questionReports, analysisMethod == AnalysisMethod.RussianAiAgent
                    ? LlmVariant.Russian
                    : LlmVariant.Foreign);

        return reportData;
    }

    private static string? Truncate(string? value, int maxLength) =>
        value == null ? null : value.Length <= maxLength ? value : value[..maxLength];
}
