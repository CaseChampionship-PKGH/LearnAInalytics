using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Analysis.Contracts.Models;

/// <summary>
/// Данные анализа
/// </summary>
public class QuestionAnalysisResult
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public QuestionReference Question { get; set; } = null!;

    /// <summary>
    /// Ответы тестируемых
    /// </summary>
    public List<ComparisonResult> Results { get; set; } = null!;
}
