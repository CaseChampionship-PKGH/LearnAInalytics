namespace LearnAInalytics.Validation.Contracts.Models.Batch.Enums;

/// <summary>
/// Пре-статус ответа
/// </summary>
public enum AnswerPreStatus
{
    /// <summary>
    /// требует анализа ИИ
    /// </summary>
    NeedAnalysis,

    /// <summary>
    /// точное совпадение с эталоном после нормализации
    /// </summary>
    ExactMatch,

    /// <summary>
    /// пустой ответ
    /// </summary>
    Empty
}
