using LearnAInalytics.Analysis.Contracts.Models.Aggregation;

namespace LearnAInalytics.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией итогового примечания для критерия программы
/// </summary>
public class CriterionAnalysis
{
    /// <summary>
    /// Название критерия
    /// </summary>
    public string CriterionName { get; set; } = string.Empty;

    /// <summary>
    /// Исходные данные
    /// </summary>
    public CriterionPromptData PromptData { get; set; } = null!;

    /// <summary>
    /// Сгенерированное примечание
    /// </summary>
    public string? Note { get; set; }
}
