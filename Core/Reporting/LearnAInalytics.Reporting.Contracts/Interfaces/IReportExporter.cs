using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting.Contracts.Interfaces;

/// <summary>
/// Экспортер отчёта
/// </summary>
public interface IReportExporter
{
    /// <summary>
    /// Тип экспортера
    /// </summary>
    ExportType ExportType { get; }

    /// <summary>
    /// Экспортировать статистику
    /// </summary>
    byte[] Export(AnalysisResult analysisData);
}
