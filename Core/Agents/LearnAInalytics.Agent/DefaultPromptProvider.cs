using System.Text;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Services.Contracts.Constants;
using LearnAInalytics.Validation.Contracts.Models.Batch;

namespace LearnAInalytics.Agent;

/// <summary>
/// <inheritdoc cref="IPromptProvider"/>
/// </summary>
public class DefaultPromptProvider : IPromptProvider
{
    string IPromptProvider.BuildBatchAnalysisPrompt(QuestionBatch batch)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Ты — эксперт по проверке тестовых заданий.");
        sb.AppendLine("Дан вопрос, эталонный ответ и список ответов тестируемых.");
        sb.AppendLine("Для каждого тестируемого определи:");
        sb.AppendLine("- процент совпадения с эталоном (0-100)");
        sb.AppendLine("- вердикт: correct / partial / incorrect");
        sb.AppendLine("- краткое обоснование (1 предложение на русском)");
        sb.AppendLine();
        sb.AppendLine($"Вопрос: {batch.Question.QuestionText}");
        sb.AppendLine($"Тип вопроса: {batch.Question.Type}");
        sb.AppendLine($"Правильный ответ: {batch.Question.CorrectAnswer}");
        sb.AppendLine("Ответы тестируемых:");

        foreach (var answer in batch.Answers)
        {
            sb.AppendLine($"{answer.UserId}: \"{answer.RawAnswer}\"");
        }

        sb.AppendLine();
        sb.AppendLine("Верни ТОЛЬКО JSON без markdown-обёртки, без пояснений:");
        sb.AppendLine("{");
        sb.AppendLine("  \"results\": [");
        sb.AppendLine("    { \"userId\": \"user_001\", \"similarityPercent\": 85, \"verdict\": \"correct\", \"comment\": \"...\" },");
        sb.AppendLine("    ...");
        sb.AppendLine("  ]");
        sb.AppendLine("}");

        return sb.ToString();
    }

    string IPromptProvider.BuildReportGeneratingPrompt(Summary summary, List<string> criticalIssues, List<QuestionReport> questions)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — методист, анализирующий результаты тестирования.");
        sb.AppendLine("Ниже представлена статистика ответов на тестовые задания.");
        sb.AppendLine($"Всего вопросов: {summary.TotalQuestions}, тестируемых: {summary.TotalUsers}.");
        sb.AppendLine($"Средний процент правильных ответов: {summary.OverallCorrectPercentage}%.");
        sb.AppendLine($"Критические вопросы (правильных ответов < {TestAnalysisConstants.PassGradePercent}%):");
        foreach (var issue in criticalIssues)
        {
            sb.AppendLine("- " + issue);
        }
        sb.AppendLine("Самые частые проблемы по вопросам:");
        foreach (var q in questions.Where(q => q.CommonMistakes.Any()))
        {
            sb.AppendLine($"Вопрос: {Truncate(q.QuestionText, 100)}");
            foreach (var m in q.CommonMistakes)
            {
                sb.AppendLine($"  - {m}");
            }
        }
        sb.AppendLine("На основе этих данных напиши рекомендации по улучшению тестовых заданий (3-5 предложений).");
        sb.AppendLine("Укажи, какие вопросы стоит пересмотреть, на что обратить внимание.");
        return sb.ToString();
    }

    private static string? Truncate(string? value, int maxLength) =>
        value == null ? null : value.Length <= maxLength ? value : value[..maxLength];
}

