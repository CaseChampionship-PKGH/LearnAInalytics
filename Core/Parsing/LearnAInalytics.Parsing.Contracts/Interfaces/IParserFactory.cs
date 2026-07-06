using LearnAInalytics.Parsing.Contracts.Enums;

namespace LearnAInalytics.Parsing.Contracts.Interfaces;

/// <summary>
/// Фабрика получения парсера
/// </summary>
public interface IParserFactory
{
    /// <summary>
    /// Получить парсер по назначению
    /// </summary>
    IDataParser GetParser(InputFormat format, ParsingTarget target);
}
