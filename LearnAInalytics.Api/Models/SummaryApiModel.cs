namespace LearnAInalytics.Api.Models;

/// <summary>
/// Сводная статистика по всем вопросам.
/// </summary>
public class SummaryApiModel
{
    /// <summary>
    /// Общее количество уникальных вопросов в тесте.
    /// </summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// Общее количество тестируемых, чьи ответы анализировались.
    /// </summary>
    public int TotalUsers { get; set; }

    /// <summary>
    /// Средний процент полностью правильных ответов (correct).
    /// </summary>
    public double OverallCorrectPercentage { get; set; }

    /// <summary>
    /// Средний процент частично правильных ответов (partial).
    /// </summary>
    public double OverallPartialPercentage { get; set; }

    /// <summary>
    /// Средний процент неправильных ответов (incorrect).
    /// </summary>
    public double OverallIncorrectPercentage { get; set; }
}
