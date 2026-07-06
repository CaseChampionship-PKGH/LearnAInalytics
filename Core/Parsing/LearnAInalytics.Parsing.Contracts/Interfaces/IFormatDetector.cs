using LearnAInalytics.Parsing.Contracts.Enums;

namespace LearnAInalytics.Parsing.Contracts.Interfaces;

/// <summary>
/// Резолвер формата данных
/// </summary>
public interface IFormatDetector
{
    /// <summary>
    /// Вычислить тип данных
    /// </summary>
    InputFormat DetectFormat(string fileName, Stream content);
}
