using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;
using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Analysis.Contracts.Interfaces;

/// <summary>
/// Cервис-преобразователь данных для промптов
/// </summary>
public interface ICriterionAggregator
{
    /// <summary>
    /// Выделить вопросы по критериям
    /// </summary>
    AggregatedCriteriaData Aggregate(Survey parsedResult, SurveyStatistics statistics);
}
