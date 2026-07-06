using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Parsing;

/// <summary>
/// <inheritdoc cref="IParserFactory"/>
/// </summary>
public class ParserFactory : IParserFactory
{
    private readonly Dictionary<(InputFormat, ParsingTarget), IDataParser> parsers;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ParserFactory"/>
    /// </summary>
    public ParserFactory(IEnumerable<IDataParser> parsers)
    {
        this.parsers = parsers.ToDictionary(
            p => (p.Format, p.Target),
            p => p
        );
    }

    IDataParser IParserFactory.GetParser(InputFormat format, ParsingTarget target)
    {
        if (parsers.TryGetValue((format, target), out var parser))
        {
            return parser;
        }

        throw new ParsingException($"Парсер для формата {format} и цели {target} не найден.");
    }
}
