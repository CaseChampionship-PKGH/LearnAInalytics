using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;

namespace LearnAInalytics.Agent.Contracts.Interfaces;

/// <summary>
/// ИИ-Агент для анализа программы по критерию
/// </summary>
public interface ISurveyCriteriaAnalysisAgent
{
    /// <summary>
    /// Проанализировать критерий
    /// </summary>
    Task<string> AnalyzeAndGenerateCriterionNoteAsync(CriterionPromptData criterionData, LlmVariant llmVariant);

    /// <summary>
    /// Проанализировать траекторию развития по всем данным
    /// </summary>
    Task<Trajectory> AnalyzeTrajectoryAsync(AggregatedCriteriaData allData, List<string> criterionNotes, LlmVariant llmVariant);
}
