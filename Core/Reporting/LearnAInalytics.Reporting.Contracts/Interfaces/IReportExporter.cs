using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting.Contracts.Interfaces;

/// <summary>
/// Экспортер отчёта
/// </summary>
public interface IReportExporter
{
    /// <summary>
    /// Экспортировать
    /// </summary>
    byte[] ExportToExcel(ReportData report);
}
