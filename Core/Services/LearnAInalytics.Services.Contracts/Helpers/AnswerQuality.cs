using System.Text.RegularExpressions;

namespace LearnAInalytics.Services.Contracts.Helpers;

/// <summary>
/// Отсеиватель неинформативныъ ответов
/// </summary>
public static partial class AnswerQuality
{
    /// <summary>
    /// Эквиваленты отсутствия ответа для отсекания всех не информативных ответов
    /// </summary>
    public static readonly HashSet<string> DashEquivalents = new(StringComparer.OrdinalIgnoreCase)
    {
        "нет ответа", "отсутствует", "отсутствуют", "прочерк", "нет ответа, все ок",
        "все норм", "все хорошо", "все отлично", "все раскрыто", "всего достаточно",
        "все супер", "достаточно по объему", "программа достаточно полная", "все актуально",
        "пока достаточно", "затрудняюсь ответить", "затрудняюсь ответить.",
        "Не готов ответить", "Не готова ответить",
        "не знаю", "не готов ответить", "не готов ответить.", "нет ответа, все ок",
        "всё раскрыто", "всего достаточно", "информации достаточно",
        "курс полон", "все полезно", "всё полезно", "все оставить", "все норм",
        "все хорошо организовано", "организация достаточна"
    };

    /// <summary>
    /// Паттерн нахождения эквивалентов прочерков
    /// </summary>
    private readonly static Regex dashPattern = DashEquivalent();

    /// <summary>
    /// Ответ не информативен?
    /// </summary>
    public static bool IsNonInformative(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        if (text.StartsWith('-') || text.StartsWith('–') || text.StartsWith('—') || text.StartsWith('_'))
        {
            return true;
        }

        if (DashEquivalents.Contains(text))
        {
            return true;
        }

        return dashPattern.IsMatch(text);
    }

    [GeneratedRegex(@"^(затрудняюсь\s*ответит[ь]?|не\s*готов[а]?\s*ответит[ь]?|не\s*готова?\s*ответит[ь]?|вс[её]\s*(норм|хорошо|отлично|раскрыто|супер|полезно|актуально)|всего\s*(достаточно|хватило)|информации\s*достаточно|программа\s*достаточно\s*полная|пока\s*достаточно|курс\s*полон|организация\s*достаточна|пожеланий\s*нет|нет\s*ответа|нет\s*пожеланий|ничего\s*менять\s*не\s*нужно)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline, "ru-RU")]
    private static partial Regex DashEquivalent();
}
