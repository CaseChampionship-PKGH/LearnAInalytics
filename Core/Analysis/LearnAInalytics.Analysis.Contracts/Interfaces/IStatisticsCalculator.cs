using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Parsing.Contracts.Models;

namespace LearnAInalytics.Analysis.Contracts.Interfaces;

/// <summary>
/// Калькулятор для подсчёта числовых и бинарных данных анекеты
/// </summary>
public interface IStatisticsCalculator
{
    /// <summary>
    /// Посчитать данные анекеты
    /// </summary>
    SurveyStatistics Calculate(SurveyParseResult parsedData);
}
