namespace LearnAInalytics.Agent.Contracts.Models;

/// <summary>
/// Сведения о ответе тестируемого
/// </summary>
public class AgentResultItem
{
    /// <summary>
    /// Идентфикатор тестируемого
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Процент схожести
    /// </summary>
    public double SimilarityPercent { get; set; }

    /// <summary>
    /// Вердикт ("correct", "partial", "incorrect")
    /// </summary>
    public string Verdict { get; set; } = string.Empty;

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
