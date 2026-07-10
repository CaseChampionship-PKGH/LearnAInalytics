using LearnAInalytics.Entities.Models;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation.Contracts.Interfaces;

/// <summary>
/// Сервис валидации данных перед анализом
/// </summary>
public interface IDataValidator
{
    /// <summary>
    /// Валидировать результаты анектирования
    /// </summary>
    ValidationResult Validate(Survey results);
}
