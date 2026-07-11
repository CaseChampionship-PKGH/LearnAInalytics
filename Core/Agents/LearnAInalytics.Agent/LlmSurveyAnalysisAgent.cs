using System.Text;
using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Agent.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Agent;

/// <inheritdoc cref="ISurveyCriteriaAnalysisAgent"/> на базе искуственного интелекта LLM
public class LlmSurveyAnalysisAgent : ISurveyCriteriaAnalysisAgent
{
    private readonly IPromptProvider promptProvider;
    private readonly IParserFactory parserFactory;
    private readonly ILlmFactory llmFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmSurveyAnalysisAgent"/>
    /// </summary>
    public LlmSurveyAnalysisAgent(IPromptProvider promptProvider,
        IParserFactory parserFactory,
        ILlmFactory llmFactory)
    {
        this.promptProvider = promptProvider;
        this.parserFactory = parserFactory;
        this.llmFactory = llmFactory;
    }

    async Task<string> ISurveyCriteriaAnalysisAgent.AnalyzeCriterionAsync(CriterionPromptData criterionData, LlmVariant llmVariant)
    {
        var prompt = promptProvider.BuildCriterionNotePrompt(criterionData);
        var llmClient = llmFactory.CreateLLmClient(llmVariant);
        var response = await llmClient.SendRequestAsync(new LlmRequest { RawPrompt = prompt }, "analysis");

        var raw = response.RawResponse;
        var cleaned = raw.Replace("```json", "").Replace("```", "").Trim();
        return cleaned;
    }

    async Task<Trajectory> ISurveyCriteriaAnalysisAgent.AnalyzeTrajectoryAsync(AggregatedCriteriaData allData, List<string> criterionNotes, LlmVariant llmVariant)
    {
        var prompt = promptProvider.BuildTrajectoryPrompt(allData, criterionNotes);
        var llmClient = llmFactory.CreateLLmClient(llmVariant);
        var response = await llmClient.SendRequestAsync(new LlmRequest { RawPrompt = prompt }, "analysis");

        var raw = response.RawResponse;
        var cleaned = raw.Replace("```json", "").Replace("```", "").Trim();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(cleaned));
        var parser = parserFactory.GetParser(InputFormat.Json, ParsingTarget.AgentResponse);

        return await parser.ParseAsync<Trajectory>(stream, string.Empty);
    }
}
