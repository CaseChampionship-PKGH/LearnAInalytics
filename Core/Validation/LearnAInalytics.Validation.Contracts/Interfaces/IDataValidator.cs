using LearnAInalytics.Entities.Models;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation.Contracts.Interfaces;

/// <summary>
/// Сервис валидации данных перед анализом
/// </summary>
public interface IDataValidator
{
    /// <summary>
    /// Валидировать результаты теста и эталонные ответы
    /// </summary>
    ValidationResult Validate(IEnumerable<SurveyResponse> results);
}
