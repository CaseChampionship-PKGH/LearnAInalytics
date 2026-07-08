using LearnAInalytics.Entities.Enums;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Interfaces;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation;

/// <summary>
/// Валидатор данных анкетирования
/// </summary>
public class DataValidator : IDataValidator
{
    private readonly static HashSet<string> dashEquivalents = new(StringComparer.OrdinalIgnoreCase)
    {
        "нет ответа", "отсутствует", "прочерк", "нет ответа, все ок",
        "все норм", "затрудняюсь ответить", "затрудняюсь ответить.",
        "не знаю", "не готов ответить", "не готов ответить.", "нет ответа, все ок"
    };

    ValidationResult IDataValidator.Validate(IEnumerable<SurveyParseResult> parsedDataList)
    {
        var warnings = new List<string>();
        var cleanedSurveys = new List<SurveyParseResult>();

        foreach (var parsedData in parsedDataList)
        {
            var cleanedResponses = new List<SurveyResponse>();

            foreach (var response in parsedData.Responses)
            {
                var cleanedAnswers = new List<SurveyAnswer>();

                foreach (var answer in response.Answers)
                {
                    var question = parsedData.Questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
                    if (question == null)
                    {
                        warnings.Add($"Ответ с ID вопроса {answer.QuestionId} не найден в справочнике.");
                        continue;
                    }

                    switch (question.Type)
                    {
                        case QuestionType.Numeric:
                            if (answer.NumericValue.HasValue)
                            {
                                if (answer.NumericValue < 1 || answer.NumericValue > 10)
                                {
                                    warnings.Add($"Оценка {answer.NumericValue} для вопроса '{question.QuestionText}' вне диапазона 1-10. Ответ будет исключён.");
                                    continue;
                                }
                                cleanedAnswers.Add(answer);
                            }
                            else
                            {
                                warnings.Add($"Пустая числовая оценка для вопроса '{question.QuestionText}'.");
                            }
                            break;

                        case QuestionType.Binary:
                            var binVal = answer.BinaryValue;
                            if (string.IsNullOrWhiteSpace(binVal))
                            {
                                warnings.Add($"Пустой бинарный ответ для вопроса '{question.QuestionText}'.");
                                continue;
                            }
                            if (!IsValidBinary(binVal))
                            {
                                if (IsDashEquivalent(binVal))
                                {
                                    warnings.Add($"Бинарный ответ '{binVal}' для вопроса '{question.QuestionText}' интерпретирован как пустой.");
                                    continue;
                                }
                                warnings.Add($"Неожиданное значение '{binVal}' для бинарного вопроса '{question.QuestionText}'. Ответ исключён.");
                                continue;
                            }
                            cleanedAnswers.Add(answer);
                            break;

                        case QuestionType.OpenText:
                            var text = answer.TextValue;
                            if (string.IsNullOrWhiteSpace(text) || IsDashEquivalent(text!))
                            {
                                continue;
                            }
                            cleanedAnswers.Add(answer);
                            break;
                    }
                }

                if (cleanedAnswers.Count > 0)
                {
                    cleanedResponses.Add(new SurveyResponse
                    {
                        RespondentId = response.RespondentId,
                        Position = response.Position,
                        Answers = cleanedAnswers
                    });
                }
                else
                {
                    warnings.Add($"Анкета респондента {response.RespondentId} полностью пуста после фильтрации.");
                }
            }

            var usedQuestionIds = cleanedResponses
                .SelectMany(r => r.Answers.Select(a => a.QuestionId))
                .Distinct()
                .ToHashSet();

            var cleanedQuestions = parsedData.Questions
                .Where(q => usedQuestionIds.Contains(q.QuestionId))
                .ToList();

            cleanedSurveys.Add(new SurveyParseResult()
            {
                ProgramInfo = parsedData.ProgramInfo,
                Questions = cleanedQuestions,
                Responses = cleanedResponses
            });
        }

        return new ValidationResult
        {
            ValidatedResults = cleanedSurveys,
            Success = cleanedSurveys.Count > 0,
            Warnings = warnings
        };
    }

    private bool IsValidBinary(string value)
    {
        return value.Equals("да", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("нет", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsDashEquivalent(string text) => text.StartsWith('-') || text.StartsWith('_') || dashEquivalents.Contains(text);
}
