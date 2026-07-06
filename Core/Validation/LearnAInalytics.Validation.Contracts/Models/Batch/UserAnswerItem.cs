using LearnAInalytics.Validation.Contracts.Models.Batch.Enums;

namespace LearnAInalytics.Validation.Contracts.Models.Batch;

/// <summary>
/// Сжатое представление вопроса для анализа
/// </summary>
public class UserAnswerItem
{
    /// <summary>
    /// Идентфикатор тестируемого
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Исходный ответ тестируемого
    /// </summary>
    public string RawAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Результат предварительной проверки
    /// </summary>
    public AnswerPreStatus PreStatus { get; set; }
}
