namespace LearnAInalytics.Api.Models;

/// <summary>
/// Результат анализа одного вопроса.
/// </summary>
public class QuestionReportApiModel
{
    /// <summary>
    /// Текст вопроса.
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Эталонный правильный ответ.
    /// </summary>
    public string CorrectAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Тип вопроса (text, code, single_choice, multiple_choice).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Общее количество ответов тестируемых на этот вопрос (после фильтрации).
    /// </summary>
    public int TotalAnswers { get; set; }

    /// <summary>
    /// Количество полностью правильных ответов (correct).
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// Количество частично правильных ответов (partial).
    /// </summary>
    public int PartialCount { get; set; }

    /// <summary>
    /// Количество неправильных ответов (incorrect).
    /// </summary>
    public int IncorrectCount { get; set; }

    /// <summary>
    /// Процент полностью правильных ответов.
    /// </summary>
    public double CorrectPercentage { get; set; }

    /// <summary>
    /// Признак критического вопроса (процент правильных ответов ниже порога).
    /// </summary>
    public bool IsCritical { get; set; }

    /// <summary>
    /// Самые частые неправильные ответы (топ-3) для выявления типовых ошибок.
    /// </summary>
    public List<string> CommonMistakes { get; set; } = new();

    /// <summary>
    /// Количество пустых ответов на этот вопрос.
    /// </summary>
    public int EmptyCount { get; set; }

    /// <summary>
    /// Средний процент совпадения с эталоном (для всех ответов).
    /// </summary>
    public double AverageSimilarityPercent { get; set; }
}
