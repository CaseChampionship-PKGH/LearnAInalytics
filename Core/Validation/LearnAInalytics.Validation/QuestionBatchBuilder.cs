using LearnAInalytics.Validation.Contracts.Interfaces;
using LearnAInalytics.Validation.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Models.Batch;

namespace LearnAInalytics.Validation;

/// <summary>
/// Валидатор данных тестов
/// </summary>
public class QuestionBatchBuilder : IQuestionBatchBuilder
{
    List<QuestionBatch> IQuestionBatchBuilder.Build(ValidationResult validationResult)
    {
        //var batches = new List<QuestionBatch>();

        //var answersByQuestion = validationResult.ValidatedResults
        //    .SelectMany(test => test.Answers, (test, answer) => new
        //    {
        //        test.UserId,
        //        answer.QuestionKey,
        //        answer.UserAnswer
        //    })
        //    .GroupBy(x => x.QuestionKey)
        //    .ToDictionary(g => g.Key, g => g.ToList());

        //foreach (var (questionKey, questionRef) in validationResult.QuestionCatalog)
        //{
        //    var answers = answersByQuestion.TryGetValue(questionKey, out var list)
        //        ? list
        //        : Enumerable.Empty<dynamic>();

        //    var userAnswerItems = new List<UserAnswerItem>();

        //    foreach (var ans in answers)
        //    {
        //        var normalizedUser = Normalize(ans.UserAnswer);
        //        var normalizedCorrect = Normalize(questionRef.CorrectAnswer);

        //        AnswerPreStatus preStatus;
        //        if (string.IsNullOrWhiteSpace(normalizedUser))
        //        {
        //            preStatus = AnswerPreStatus.Empty;
        //        }
        //        else if (normalizedUser == normalizedCorrect)
        //        {
        //            preStatus = AnswerPreStatus.ExactMatch;
        //        }
        //        else
        //        {
        //            preStatus = AnswerPreStatus.NeedAnalysis;
        //        }

        //        userAnswerItems.Add(new UserAnswerItem
        //        {
        //            UserId = ans.UserId,
        //            RawAnswer = ans.UserAnswer,
        //            PreStatus = preStatus
        //        });
        //    }

        //    batches.Add(new QuestionBatch
        //    {
        //        Question = questionRef,
        //        Answers = userAnswerItems
        //    });
        //}

        //return batches;

        return [];
    }

    private string Normalize(string text) =>
        text?.Trim() ?? string.Empty;
}
