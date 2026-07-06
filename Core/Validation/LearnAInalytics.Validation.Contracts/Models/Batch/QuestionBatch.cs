namespace LearnAInalytics.Validation.Contracts.Models.Batch;

/// <summary>
/// Результат группировки для одного вопроса
/// </summary>
public record QuestionBatch
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public QuestionReference Question { get; set; } = null!;

    /// <summary>
    /// Ответы тестируемых
    /// </summary>
    public List<UserAnswerItem> Answers { get; set; } = null!;
}
