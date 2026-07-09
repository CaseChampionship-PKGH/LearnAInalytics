using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Validation.Contracts.Models;

/// <summary>
/// Сжатое представление открытых ответов на вопрос для анализа
/// </summary>
public record QuestionWithAnswers
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public SurveyQuestion Question { get; set; } = null!;

    /// <summary>
    /// Ответы
    /// </summary>
    public List<SurveyAnswer> Answers { get; set; } = null!;
}
