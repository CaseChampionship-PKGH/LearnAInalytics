using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting.Contracts.Interfaces;

/// <summary>
/// Фабрика экспортеров отчёта
/// </summary>
public interface IReportExporterFactory
{
    /// <summary>
    /// Получить экспортер
    /// </summary>
    IReportExporter GetReportExporter(ExportType type);
}
