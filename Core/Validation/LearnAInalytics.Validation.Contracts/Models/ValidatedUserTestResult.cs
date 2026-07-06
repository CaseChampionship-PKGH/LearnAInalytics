namespace LearnAInalytics.Validation.Contracts.Models;

/// <summary>
/// Результат прохождения теста
/// </summary>
public class ValidatedUserTestResult
{
    /// <summary>
    /// Идентификатор тестируемого
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Время начала
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Статус теста (пройден/не пройден)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Количество баллов
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Максимальное количество баллов
    /// </summary>
    public int MaxScore { get; set; }

    /// <summary>
    /// Ответы тестируемого
    /// </summary>
    public List<ValidatedQuestionAnswer> Answers { get; set; } = null!;
}
