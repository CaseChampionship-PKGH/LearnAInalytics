using LearnAInalytics.Agent.Contracts.Enums;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Agent;

/// <inheritdoc cref="IReportAgent"/> на базе искуственного интелекта LLM
public class LlmReportAgent : IReportAgent
{
    private readonly IPromptProvider promptProvider;
    private readonly ILlmFactory llmFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LlmReportAgent"/>
    /// </summary>
    public LlmReportAgent(IPromptProvider promptProvider,
        ILlmFactory llmFactory)
    {
        this.promptProvider = promptProvider;
        this.llmFactory = llmFactory;
    }

    async Task<ReportData> IReportAgent.GenerateReportAsync(Summary summary, List<string> criticalIssues, List<QuestionReport> questions, LlmVariant llmVariant)
    {
        //var prompt = promptProvider.BuildReportGeneratingPrompt(summary, criticalIssues, questions);
        //var llmClient = llmFactory.CreateLLmClient(llmVariant);
        //var recommendations = await llmClient.SendRequestAsync(new LlmRequest()
        //{
        //    RawPrompt = prompt
        //}, "report");

        //return new ReportData
        //{
        //    Summary = summary,
        //    Questions = questions,
        //    CriticalIssues = criticalIssues,
        //    Recommendations = recommendations.RawResponse
        //};

        return new ReportData();
    }
}
