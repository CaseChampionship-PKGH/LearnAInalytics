using LearnAInalytics.Analysis.Contracts.Enums;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting.Contracts.Interfaces;

/// <summary>
/// Генератор отчёта
/// </summary>
public interface IReportBuilder
{
    /// <summary>
    /// Построить отчёт по анализам
    /// </summary>
    Task<ReportData> BuildAsync(List<QuestionAnalysisResult> data, AnalysisMethod analysisMethod);
}
