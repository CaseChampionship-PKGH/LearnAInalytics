namespace LearnAInalytics.Analysis.Contracts.Models;

/// <summary>
/// Результат сравнения тестирования
/// </summary>
public class ComparisonResult
{
    /// <summary>
    /// Идентфикатор тестируемого
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Вердикт ("correct", "partial", "incorrect")
    /// </summary>
    public string Verdict { get; set; } = string.Empty;

    /// <summary>
    /// Процент равности
    /// </summary>
    public double SimilarityPercent { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
