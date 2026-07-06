namespace LearnAInalytics.Entities.Models;

/// <summary>
/// Ответ на вопрос
/// </summary>
public class QuestionAnswer
{
    /// <summary>
    /// Текст вопроса
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Тип вопроса
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Ответ тестируемого
    /// </summary>
    public string UserAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Правильный ответ
    /// </summary>
    public string CorrectAnswer { get; set; } = string.Empty;
}
