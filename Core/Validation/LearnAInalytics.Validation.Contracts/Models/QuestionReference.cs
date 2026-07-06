namespace LearnAInalytics.Validation.Contracts.Models;

/// <summary>
/// Сжатое представление вопроса для анализа
/// </summary>
public record QuestionReference
{
    /// <summary>
    /// Текст вопроса
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Правильный ответ
    /// </summary>
    public string CorrectAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Тип вопроса
    /// </summary>
    public string Type { get; set; } = string.Empty;
}
