using LearnAInalytics.Analysis.Contracts.Models;

namespace LearnAInalytics.Services.Contracts.Models;

/// <summary>
/// Результат выполнения pipeline
/// </summary>
public record PipelineResult
{
    ///// <summary>
    ///// Отчёт
    ///// </summary>
    //public ReportData ReportData { get; set; } = null!;

    ///// <summary>
    ///// Результат анализа
    ///// </summary>
    //public byte[] ExcelReport { get; set; } = null!;

    /// <summary>
    /// Список возникших в процессе валидации ошибок
    /// </summary>
    public List<string> Errors { get; set; } = null!;

    /// <summary>
    /// Статистика
    /// </summary>
    public AnalysisResult AnalysisResult { get; set; } = null!;
}
