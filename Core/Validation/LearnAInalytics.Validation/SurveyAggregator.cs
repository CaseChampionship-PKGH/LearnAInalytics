using LearnAInalytics.Parsing.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Interfaces;
using LearnAInalytics.Validation.Contracts.Models;

namespace LearnAInalytics.Validation;

/// <inheritdoc cref="ISurveyAggregator"/>
public class SurveyAggregator : ISurveyAggregator
{
    List<QuestionWithAnswers> ISurveyAggregator.Aggregate(SurveyParseResult parsedResult)
    {
        var questionDict = parsedResult.Questions.ToDictionary(q => q.QuestionId);
        var answersByQuestion = parsedResult.Responses
            .SelectMany(r => r.Answers)
            .Where(a => questionDict.ContainsKey(a.QuestionId))
            .GroupBy(a => a.QuestionId)
            .Select(g => new QuestionWithAnswers
            {
                Question = questionDict[g.Key],
                Answers = g.ToList()
            })
            .ToList();
        return answersByQuestion;
    }
}
