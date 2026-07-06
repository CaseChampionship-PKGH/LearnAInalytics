using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа правильности ответа
/// </summary>
public interface IReportAgent
{
    /// <summary>
    /// Сгенерировать отчёт по данным анализов 
    /// </summary>
    Task<ReportData> GenerateReportAsync(Summary summary, List<string> criticalIssues, List<QuestionReport> questions, LlmVariant llmVariant);
}
