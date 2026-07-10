using System.Text;
using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Agent.Contracts.Models;
using LearnAInalytics.Analysis.Contracts.Models.Aggregation;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Interfaces;

namespace LearnAInalytics.Agent;

/// <inheritdoc cref="ITestAnalysisAgent"/> на базе искуственного интелекта LLM
public class LlmTestAnalysisAgent : ITestAnalysisAgent
{
    private readonly IPromptProvider promptProvider;
    private readonly IParserFactory parserFactory;
    private readonly ILlmFactory llmFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmTestAnalysisAgent"/>
    /// </summary>
    public LlmTestAnalysisAgent(IPromptProvider promptProvider,
        IParserFactory parserFactory,
        ILlmFactory llmFactory)
    {
        this.promptProvider = promptProvider;
        this.parserFactory = parserFactory;
        this.llmFactory = llmFactory;
    }

    async Task<AgentBatchResponse> ITestAnalysisAgent.AnalyzeBatchAsync(AggregatedCriteriaData aggregatedCriteriaData, LlmVariant llmVariant)
    {
        var prompt = promptProvider.BuildBatchAnalysisPrompt(aggregatedCriteriaData);
        var llmClient = llmFactory.CreateLLmClient(llmVariant);
        var rawResponse = await llmClient.SendRequestAsync(new LlmRequest()
        {
            RawPrompt = prompt
        }, "analysis");

        var cleanJson = rawResponse.RawResponse
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(cleanJson));
        var parser = parserFactory.GetParser(InputFormat.Json, ParsingTarget.AgentResponse);
        return await parser.ParseAsync<AgentBatchResponse>(stream, string.Empty);
    }
}
