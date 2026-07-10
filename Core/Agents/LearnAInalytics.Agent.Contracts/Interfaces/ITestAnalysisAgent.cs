using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;

namespace LearnAInalytics.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа правильности ответа
/// </summary>
public interface ITestAnalysisAgent
{
    /// <summary>
    /// Сравнить правильность ответа тестируемого
    /// </summary>
    Task<AgentBatchResponse> AnalyzeBatchAsync(AggregatedCriteriaData aggregatedCriteriaData, LlmVariant llmVariant);
}
