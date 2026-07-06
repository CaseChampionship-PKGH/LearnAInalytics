namespace LearnAInalytics.Agent.Contracts.Models;

/// <summary>
/// Полученный анализ ИИ-агента на один вопрос
/// </summary>
public class AgentBatchResponse
{
    /// <summary>
    /// Результаты
    /// </summary>
    public List<AgentResultItem> Results { get; set; } = null!;
}
