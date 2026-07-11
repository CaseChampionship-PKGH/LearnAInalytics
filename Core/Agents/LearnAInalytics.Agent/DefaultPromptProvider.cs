using System.Text;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;

namespace LearnAInalytics.Agent;

/// <inheritdoc cref="IPromptProvider"/>
public class DefaultPromptProvider : IPromptProvider
{
    string IPromptProvider.BuildCriterionNotePrompt(CriterionPromptData criterionData)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — аналитик образовательных программ.");
        sb.AppendLine($"Проанализируй ответы слушателей по критерию «{criterionData.CriterionName}».");
        sb.AppendLine();

        // Статистика
        var stats = criterionData.Statistics;
        if (stats != null)
        {
            if (stats.Average.HasValue)
            {
                sb.AppendLine($"Статистика: средний балл — {stats.Average:F1} из 10.");
                if (stats.Distribution != null)
                {
                    sb.AppendLine($"Распределение оценок: 1-3: {stats.PercentLow:F1}%, " +
                                  $"4-7: {stats.PercentMedium:F1}%, " +
                                  $"8-10: {stats.PercentHigh:F1}%.");
                }
            }
            else if (stats.YesCount.HasValue)
            {
                sb.AppendLine($"Статистика: \"Да\" — {stats.YesCount} чел. ({stats.YesPercent:F1}%), " +
                              $"\"Нет\" — {stats.NoCount} чел. ({stats.NoPercent:F1}%).");
            }
        }

        // Ответы на вопросы
        sb.AppendLine();
        sb.AppendLine("Ответы на вопросы:");
        foreach (var qa in criterionData.Questions)
        {
            if (qa.Answers.Count == 0)
            {
                continue;
            }

            sb.AppendLine($"Вопрос: «{qa.Question.QuestionText}»");
            foreach (var ans in qa.Answers)
            {
                sb.AppendLine($"- «{ans}»");
            }

            sb.AppendLine();
        }

        sb.AppendLine("На основе этих данных напиши Примечание на 3-8 предложений деловым стилем. " +
                      "Указывай точные цифры. Не придумывай факты, опирайся только на предоставленные ответы.");
        return sb.ToString();
    }


    string IPromptProvider.BuildTrajectoryPrompt(AggregatedCriteriaData allData, List<string> notes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Ты — методист образовательных программ.");
        sb.AppendLine("Составь раздел «Траектория изменения программы» на основе следующих данных:");
        sb.AppendLine();

        var criterionNames = new[] { "Полезность", "Практико-ориентированность", "Доступность", "Взаимодействие с КУ", "Вовлеченность" };
        for (var i = 0; i < criterionNames.Length && i < notes.Count; i++)
        {
            sb.AppendLine($"Примечание по критерию «{criterionNames[i]}»: {notes[i]}");
            sb.AppendLine();
        }

        sb.AppendLine("Сводная статистика:");
        foreach (var c in allData.AllCriteriaData)
        {
            var s = c.Statistics;
            if (s == null)
            {
                continue;
            }

            if (s.Average.HasValue)
            {
                sb.AppendLine($"- {c.CriterionName}: средний балл {s.Average:F1}");
            }
            else if (s.YesCount.HasValue)
            {
                sb.AppendLine($"- {c.CriterionName}: Да — {s.YesCount}, Нет — {s.NoCount} ({s.YesPercent:F1}%)");
            }
        }

        if (allData.ExcludedTopics.Count > 0)
        {
            sb.AppendLine("Предложения исключить темы:");
            foreach (var t in allData.ExcludedTopics)
            {
                sb.AppendLine($"- {t}");
            }
        }
        if (allData.SuggestedTopics.Count > 0)
        {
            sb.AppendLine("Предложения добавить темы:");
            foreach (var t in allData.SuggestedTopics)
            {
                sb.AppendLine($"- {t}");
            }
        }

        var dist = allData.FormatDistribution;
        var total = dist.FullTime + dist.Mixed + dist.Remote;
        sb.AppendLine($"Предпочтения по форме обучения: очно — {dist.FullTime} чел. ({(total > 0 ? 100.0 * dist.FullTime / total : 0):F1}%), " +
                      $"смешанно — {dist.Mixed} чел. ({(total > 0 ? 100.0 * dist.Mixed / total : 0):F1}%), " +
                      $"дистанционно — {dist.Remote} чел. ({(total > 0 ? 100.0 * dist.Remote / total : 0):F1}%).");

        sb.AppendLine();
        sb.AppendLine("Сформулируй раздел, состоящий из пунктов:");
        sb.AppendLine("1. Потребность в дальнейшей реализации программы (используй средний балл полезности).");
        sb.AppendLine("2. Корректировка отбора слушателей.");
        sb.AppendLine("3. Дополнение программы учебными вопросами.");
        sb.AppendLine("4. Изменение количества часов в программе.");
        sb.AppendLine("5. Изменение формы обучения (проанализируй распределение).");
        sb.AppendLine("Каждый пункт — 1-2 предложения деловым стилем, без вводных слов. " +
                      "Если по пункту нет оснований для изменений, пиши: \"Не требуется\" или \"Нет необходимости\".");
        sb.AppendLine("Верни ответ строго в формате JSON:");
        sb.AppendLine("{");
        sb.AppendLine("  \"needForProgram\": \"...\",");
        sb.AppendLine("  \"admissionCorrection\": \"...\",");
        sb.AppendLine("  \"programSupplement\": \"...\",");
        sb.AppendLine("  \"hoursChange\": \"...\",");
        sb.AppendLine("  \"formChange\": \"...\"");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

