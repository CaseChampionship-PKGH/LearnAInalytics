using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Models.Batch;

namespace LearnAInalytics.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа правильности ответа
/// </summary>
public interface ITestAnalysisAgent
{
    /// <summary>
    /// Сравнить правильность ответа тестируемого
    /// </summary>
    Task<AgentBatchResponse> AnalyzeBatchAsync(QuestionBatch questionBatch, LlmVariant llmVariant);
}
