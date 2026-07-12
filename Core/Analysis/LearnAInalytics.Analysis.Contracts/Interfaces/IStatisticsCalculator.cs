using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Analysis.Contracts.Interfaces;

/// <summary>
/// Калькулятор для подсчёта числовых и бинарных данных анекеты
/// </summary>
public interface IStatisticsCalculator
{
    /// <summary>
    /// Посчитать данные анекеты
    /// </summary>
    SurveyStatistics Calculate(Survey parsedData);

    /// <summary>
    /// Посчитать данные одного вопроса
    /// </summary>
    QuestionStatistics? CalculateForQuestion(SurveyQuestion? question, IEnumerable<SurveyResponse> responses);
}
