using LearnAInalytics.Parsing.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation.Contracts.Interfaces;

/// <summary>
/// Cервис-преобразователь данных для анализа
/// </summary>
public interface ISurveyAggregator
{
    /// <summary>
    /// Перестроить данные от «тестируемый-центричного» вида к «вопрос-центричным»
    /// </summary>
    List<QuestionWithAnswers> Aggregate(SurveyParseResult parsedResult);
}
