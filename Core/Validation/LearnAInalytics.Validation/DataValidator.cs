using LearnAInalytics.Entities.Models;
using LearnAInalytics.Validation.Contracts.Interfaces;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation;

/// <summary>
/// Валидатор данных тестов
/// </summary>
public class DataValidator : IDataValidator
{
    ValidationResult IDataValidator.Validate(IEnumerable<SurveyResponse> results)
    {
        //var list = results.ToList();
        //if (list.Count == 0)
        //{
        //    throw new ValidationException("Нет данных для валидации.");
        //}

        //var warnings = new List<string>();

        ////var nonZero = list.Where(r => r.Score > 0).ToList();
        //var nonZero = list.ToList();
        //var removedZero = list.Count - nonZero.Count;
        //if (removedZero > 0)
        //{
        //    warnings.Add($"Удалено {removedZero} тестов с нулевым баллом.");
        //}

        //if (!nonZero.Any())
        //{
        //    throw new ValidationException("Все тесты имеют нулевой балл.");
        //}

        //var latest = nonZero
        //    .GroupBy(r => r.UserId)
        //    .Select(g => g.OrderByDescending(r => r.Date).First())
        //    .ToList();

        //var duplicates = nonZero.Count - latest.Count;
        //if (duplicates > 0)
        //{
        //    warnings.Add($"Удалено {duplicates} повторных прохождений (оставлены последние).");
        //}

        //var catalog = new Dictionary<string, QuestionReference>();
        //foreach (var test in latest)
        //{
        //    foreach (var a in test.Answers)
        //    {
        //        var key = $"{a.QuestionText}|{a.CorrectAnswer}";
        //        if (!catalog.ContainsKey(key))
        //        {
        //            catalog[key] = new QuestionReference
        //            {
        //                QuestionText = a.QuestionText,
        //                CorrectAnswer = a.CorrectAnswer,
        //                Type = a.Type
        //            };
        //        }
        //    }
        //}

        //var missing = latest.SelectMany(t => t.Answers)
        //                    .Where(a => string.IsNullOrWhiteSpace(a.CorrectAnswer))
        //                    .ToList();
        //if (missing.Count != 0)
        //{
        //    throw new ValidationException($"У {missing.Count} ответов отсутствует правильный ответ.");
        //}

        //var validated = latest.Select(test => new ValidatedUserTestResult
        //{
        //    UserId = test.UserId,
        //    Date = test.Date,
        //    Status = test.Status,
        //    Score = test.Score,
        //    MaxScore = test.MaxScore,
        //    Answers = test.Answers.Select(a => new ValidatedQuestionAnswer
        //    {
        //        QuestionKey = $"{a.QuestionText}|{a.CorrectAnswer}",
        //        UserAnswer = a.UserAnswer
        //    }).ToList()
        //}).ToList();

        return new ValidationResult
        {
            Success = true,
            Warnings = ["asdasd"],
            ValidatedResults = [],
            QuestionCatalog = []
        };
    }
}
