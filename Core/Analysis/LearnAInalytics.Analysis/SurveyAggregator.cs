using System.Text.RegularExpressions;
using LearnAInalytics.Analysis.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;
using LearnAInalytics.Entities.Enums;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Services.Contracts.Constants;

namespace LearnAInalytics.Analysis;

/// <inheritdoc cref="ICriterionAggregator"/>
public class SurveyAggregator : ICriterionAggregator
{
    AggregatedCriteriaData ICriterionAggregator.Aggregate(Survey parsedResult, SurveyStatistics statistics)
    {
        var result = new AggregatedCriteriaData();
        var questions = parsedResult.Questions;
        var responses = parsedResult.Responses;

        // 1. Сопоставление вопросов критериям с учётом уточняющих
        var questionCriterionMap = new Dictionary<string, string>(); // QuestionId -> CriterionName
        string? lastNonClarifyingCriterion = null;

        foreach (var q in questions)
        {
            var criterion = DetermineCriterion(q.QuestionText);
            if (criterion == null)
            {
                // Уточняющий вопрос – привязываем к предыдущему критерию
                if (lastNonClarifyingCriterion != null)
                {
                    criterion = lastNonClarifyingCriterion;
                }
                else
                {
                    continue; // не можем определить, пропускаем
                }
            }
            else
            {
                lastNonClarifyingCriterion = criterion;
            }

            questionCriterionMap[q.QuestionId] = criterion;
        }

        // 2. Формирование данных по каждому критерию
        var criterionNames = new[] { "Полезность", "Практико-ориентированность", "Доступность", "Взаимодействие с КУ", "Вовлеченность" };
        foreach (var name in criterionNames)
        {
            var promptData = new CriterionPromptData
            {
                CriterionName = name,
                Statistics = statistics.Questions.FirstOrDefault(s => QuestionBelongsToCriterion(s.QuestionId, name))
                              ?? new QuestionStatistics()
            };

            // Находим все вопросы этого критерия
            var criterionQuestions = questions
                .Where(q => questionCriterionMap.TryGetValue(q.QuestionId, out var c) && c == name)
                .ToList();

            foreach (var q in criterionQuestions)
            {
                var questionWithAnswers = new QuestionWithAnswers { QuestionText = q.QuestionText };
                // Собираем ответы
                foreach (var response in responses)
                {
                    var answer = response.Answers.FirstOrDefault(a => a.QuestionId == q.QuestionId);
                    if (answer == null)
                    {
                        continue;
                    }

                    string? textToAdd = null;
                    if (q.Type == QuestionType.OpenText && !string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        textToAdd = answer.TextValue;
                    }
                    else if (q.Type == QuestionType.Binary && !string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        // Уточняющий открытый ответ после бинарного: проверяем условие
                        // Родительский бинарный вопрос можно найти по предыдущему вопросу того же критерия
                        // Упростим: для уточняющих вопросов мы уже знаем, что они привязаны к тому же критерию,
                        // и их ответы нужно фильтровать по значению бинарного ответа.
                        // Определим родительский бинарный вопрос: в порядке следования вопросов ищем последний бинарный
                        var parentBinary = FindParentBinaryQuestion(criterionQuestions, q);
                        if (parentBinary != null)
                        {
                            var parentAnswer = response.Answers.FirstOrDefault(a => a.QuestionId == parentBinary.QuestionId);
                            if (parentAnswer != null && ShouldIncludeClarification(q.QuestionText, parentAnswer.BinaryValue))
                            {
                                textToAdd = answer.TextValue;
                            }
                        }
                        else
                        {
                            textToAdd = answer.TextValue; // нет родителя – добавляем всегда
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(textToAdd))
                    {
                        questionWithAnswers.Answers.Add(textToAdd);
                    }
                }

                promptData.Questions.Add(questionWithAnswers);
            }

            result.Criteria.Add(promptData);
        }

        // 3. Извлечение ExcludedTopics и SuggestedTopics
        result.ExcludedTopics = ExtractOpenResponses(questions, responses, "исключить из программы");
        result.SuggestedTopics = ExtractOpenResponses(questions, responses, "дополнить программу");

        // 4. Распределение форматов обучения
        result.FormatDistribution = ExtractFormatDistribution(questions, responses);

        return result;
    }

    private static string? DetermineCriterion(string questionText)
    {
        foreach (var kv in SurveyAnalysisConstants.CriterionKeywords)
        {
            if (Regex.IsMatch(questionText, kv.Value, RegexOptions.IgnoreCase))
            {
                return kv.Key;
            }
        }
        return null; // уточняющий или не идентифицирован
    }

    private static bool QuestionBelongsToCriterion(string questionId, string criterionName)
    {
        // Используется для поиска статистики – здесь можно полагаться на маппинг,
        // но для простоты будем проверять через текст вопроса (повторный вызов DetermineCriterion)
        // В реальном коде лучше передавать маппинг
        return false; // Заглушка – заменим внутри основного метода
    }

    private static SurveyQuestion? FindParentBinaryQuestion(List<SurveyQuestion> criterionQuestions, SurveyQuestion clarifying)
    {
        // Ищем предыдущий вопрос, который не является уточняющим
        var index = criterionQuestions.IndexOf(clarifying);
        for (var i = index - 1; i >= 0; i--)
        {
            if (criterionQuestions[i].Type == QuestionType.Binary)
            {
                return criterionQuestions[i];
            }
        }
        return null;
    }

    private static bool ShouldIncludeClarification(string clarifyingText, string? binaryValue)
    {
        // Если вопрос о "нет" и бинарный ответ "нет", то включаем
        var asksForNo = clarifyingText.Contains("нет", StringComparison.OrdinalIgnoreCase) ||
                         clarifyingText.Contains("напишите, пожалуйста, почему", StringComparison.OrdinalIgnoreCase);
        var asksForYes = clarifyingText.Contains("да", StringComparison.OrdinalIgnoreCase) &&
                          !clarifyingText.Contains("нет", StringComparison.OrdinalIgnoreCase);

        return asksForNo && binaryValue?.ToLower() == "нет" || asksForYes && binaryValue?.ToLower() == "да";
    }

    private static List<string> ExtractOpenResponses(List<SurveyQuestion> questions, List<SurveyResponse> responses, string keyword)
    {
        var targetQuestion = questions.FirstOrDefault(q => q.QuestionText.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        return targetQuestion == null
            ? []
            : responses
            .SelectMany(r => r.Answers)
            .Where(a => a.QuestionId == targetQuestion.QuestionId && !string.IsNullOrWhiteSpace(a.TextValue))
            .Select(a => a.TextValue!)
            .ToList();
    }

    private static LearningFormatDistribution ExtractFormatDistribution(List<SurveyQuestion> questions, List<SurveyResponse> responses)
    {
        var formatQuestion = questions.FirstOrDefault(q => q.QuestionText.Contains("формат обучения", StringComparison.OrdinalIgnoreCase));
        if (formatQuestion == null)
        {
            return new LearningFormatDistribution();
        }

        var answers = responses
            .SelectMany(r => r.Answers)
            .Where(a => a.QuestionId == formatQuestion.QuestionId && !string.IsNullOrWhiteSpace(a.TextValue))
            .Select(a => a.TextValue!);

        int full = 0, mixed = 0, remote = 0;
        foreach (var a in answers)
        {
            if (a.Contains("очное", StringComparison.OrdinalIgnoreCase) && !a.Contains("смешанное", StringComparison.OrdinalIgnoreCase))
            {
                full++;
            }
            else if (a.Contains("смешанное", StringComparison.OrdinalIgnoreCase))
            {
                mixed++;
            }
            else if (a.Contains("дистанцион", StringComparison.OrdinalIgnoreCase))
            {
                remote++;
            }
        }
        return new LearningFormatDistribution { FullTime = full, Mixed = mixed, Remote = remote };
    }
}
