using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Parsing.Contracts.Exceptions;

namespace LearnAInalytics.Agent;

/// <summary>
/// <inheritdoc cref="ILlmFactory"/>
/// </summary>
public class LlmFactory : ILlmFactory
{
    private readonly Dictionary<LlmVariant, ILlmClient> llmClients;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmFactory"/>
    /// </summary>
    public LlmFactory(IEnumerable<ILlmClient> llmClients)
    {
        this.llmClients = llmClients.ToDictionary(
            p => p.LlmVariant,
            p => p
        );
    }

    ILlmClient ILlmFactory.CreateLLmClient(LlmVariant variant)
    {
        if (llmClients.TryGetValue(variant, out var client))
        {
            return client;
        }

        throw new ParsingException($"ИИ-агент группы {variant} не найден.");
    }
}
