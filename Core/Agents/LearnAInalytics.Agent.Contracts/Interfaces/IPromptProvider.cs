using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Models.Batch;

namespace LearnAInalytics.Agent.Contracts.Interfaces;

/// <summary>
/// Помошник в преобразовании данных в промпт
/// </summary>
public interface IPromptProvider
{
    /// <summary>
    /// Преобразовать данные одного вопроса в промпт анализа
    /// </summary>
    string BuildBatchAnalysisPrompt(QuestionBatch batch);

    /// <summary>
    /// Преобразовать данные одного вопроса в промпт создания отчёта
    /// </summary>
    string BuildReportGeneratingPrompt(Summary summary, List<string> criticalIssues, List<QuestionReport> questions);
}
