namespace LearnAInalytics.Validation.Contracts.Models;

/// <summary>
/// Результат валидации
/// </summary>
public record ValidationResult
{
    /// <summary>
    /// Список найдённых ошибок
    /// </summary>
    public IEnumerable<string> Warnings { get; set; } = [];

    /// <summary>
    /// Успешно ли прошла валидация
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Валидные данные, готовые к анализу
    /// </summary>
    public IEnumerable<ValidatedUserTestResult> ValidatedResults { get; set; } = null!;

    /// <summary>
    /// Уникальный справочник вопросов (ключ = текст + | + правильный ответ)
    /// </summary>
    public Dictionary<string, QuestionReference> QuestionCatalog { get; set; } = null!;
}
