using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Api.Models;

/// <summary>
/// Итоговый отчёт анализа тестовых заданий.
/// </summary>
public class ReportDataApiModel
{
    /// <summary>
    /// Общая сводка по результатам анализа.
    /// </summary>
    public SummaryApiModel Summary { get; set; } = new();

    /// <summary>
    /// Детальная информация по каждому вопросу.
    /// </summary>
    public List<QuestionReportApiModel> Questions { get; set; } = new();

    /// <summary>
    /// Критические изменения и проблемы, выявленные в ходе анализа.
    /// </summary>
    public List<string> CriticalIssues { get; set; } = new();

    /// <summary>
    /// Рекомендации по улучшению тестовых заданий (генерируются ИИ-агентом).
    /// </summary>
    public string Recommendations { get; set; } = string.Empty;

    /// <summary>
    /// Распарсенные данные
    /// </summary>
    public List<UserTestResult> ParsedUsers { get; set; } = null!;
}
