using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Reporting.Contracts.Models;

/// <summary>
/// Итоговый отчёт анализа тестовых заданий.
/// </summary>
public class ReportData
{
    /// <summary>
    /// Общая сводка по результатам анализа.
    /// </summary>
    public Summary Summary { get; set; } = null!;

    /// <summary>
    /// Детальная информация по каждому вопросу.
    /// </summary>
    public List<QuestionReport> Questions { get; set; } = null!;

    /// <summary>
    /// Критические изменения и проблемы, выявленные в ходе анализа.
    /// </summary>
    public List<string> CriticalIssues { get; set; } = null!;

    /// <summary>
    /// Рекомендации по улучшению тестовых заданий (генерируются ИИ-агентом).
    /// </summary>
    public string Recommendations { get; set; } = string.Empty;

    /// <summary>
    /// Распарсенные данные
    /// </summary>
    public List<SurveyResponse> ParsedUsers { get; set; } = null!;
}
