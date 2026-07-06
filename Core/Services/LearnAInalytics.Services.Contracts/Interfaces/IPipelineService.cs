using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Services.Contracts.Models;

namespace LearnAInalytics.Services.Contracts.Interfaces;

/// <summary>
/// Сервис - оркестратор: вызывает последовательно парсинг, валидацию, временной анализ, агентов сравнения, сбор статистики и генерацию отчёта.
/// </summary>
public interface IPipelineService
{
    /// <summary>
    /// Валидировать результаты теста
    /// </summary>
    Task<PipelineResult> RunAsync(PipelineContext context);

    /// <summary>
    /// Экспортировать отчёт в Excel
    /// </summary>
    Task<byte[]> ExportReportExcel(ReportData reportData);
}
