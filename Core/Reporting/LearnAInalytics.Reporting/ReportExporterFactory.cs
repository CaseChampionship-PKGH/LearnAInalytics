using LearnAInalytics.Reporting.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting;

/// <inheritdoc cref="IReportExporterFactory"/>
public class ReportExporterFactory : IReportExporterFactory
{
    private readonly Dictionary<ExportType, IReportExporter> reportExporters;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ReportExporterFactory"/>
    /// </summary>
    public ReportExporterFactory(IEnumerable<IReportExporter> reportExporters)
    {
        this.reportExporters = reportExporters.ToDictionary(
            p => p.ExportType,
            p => p
        );
    }

    IReportExporter IReportExporterFactory.GetReportExporter(ExportType type)
    {
        if (reportExporters.TryGetValue(type, out var reportExporter))
        {
            return reportExporter;
        }

        throw new NotImplementedException($"Экспортер {type} не найден.");
    }
}
