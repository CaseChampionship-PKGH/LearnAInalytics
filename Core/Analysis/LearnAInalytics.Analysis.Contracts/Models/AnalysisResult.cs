using LearnAInalytics.Analysis.Contracts.Models.Aggregation;
using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Analysis.Contracts.Models;

/// <summary>
/// Результат анализа с генерацией итогового примечания для критерия программы
/// </summary>
public class AnalysisResult
{
    /// <summary>
    /// Метаданные программы полученные из SurveyParseResult
    /// </summary>
    public ProgramInfo ProgramInfo { get; set; } = null!;

    /// <summary>
    /// Результат анализа с генерацией итогового примечания для каждого критерия программы
    /// </summary>
    public List<CriterionAnalysis> AllCriteriaAnalysisData { get; set; } = null!;

    /// <summary>
    /// Темы, предложенные к исключению (на основе вопроса "Какие темы можно исключить")
    /// </summary>
    public List<string> ExcludedTopics { get; set; } = [];

    /// <summary>
    /// Темы, предложенные к добавлению (на основе вопроса "Какими темами дополнить")
    /// </summary>
    public List<string> SuggestedTopics { get; set; } = [];

    /// <summary>
    /// Предпочтительная форма обучения
    /// </summary>
    public LearningFormatDistribution FormatDistribution { get; set; } = null!;

    /// <summary>
    /// Траектория изменения программы по результатам итогового опроса слушателей
    /// </summary>
    public Trajectory? Trajectory { get; set; }

}
