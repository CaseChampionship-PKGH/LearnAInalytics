using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Contracts.Models;
using LearnAInalytics.Reporting.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Services.Contracts.Interfaces;
using LearnAInalytics.Services.Contracts.Models;
using LearnAInalytics.Validation.Contracts.Interfaces;

namespace LearnAInalytics.Services;

/// <summary>
/// <inheritdoc cref="IPipelineService"/>
/// </summary>
public class SurveyAnalysisPipeline : IPipelineService
{
    private readonly IFormatDetector formatDetector;
    private readonly IParserFactory parserFactory;
    private readonly IDataValidator dataValidator;
    private readonly IStatisticsCalculator statisticsCalculator;
    private readonly IQuestionBatchBuilder batchBuilder;
    private readonly ITestAnalysisAgent testAnalysisAgent;
    private readonly IReportBuilder reportBuilder;
    private readonly IReportExporter reportExporter;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAnalysisPipeline"/>
    /// </summary>
    public SurveyAnalysisPipeline(IFormatDetector formatDetector,
        IParserFactory parserFactory,
        IDataValidator dataValidator,
        IStatisticsCalculator statisticsCalculator,
        IQuestionBatchBuilder batchBuilder,
        ITestAnalysisAgent testAnalysisAgent,
        IReportBuilder reportBuilder,
        IReportExporter reportExporter)
    {
        this.formatDetector = formatDetector;
        this.parserFactory = parserFactory;
        this.dataValidator = dataValidator;
        this.statisticsCalculator = statisticsCalculator;
        this.batchBuilder = batchBuilder;
        this.testAnalysisAgent = testAnalysisAgent;
        this.reportBuilder = reportBuilder;
        this.reportExporter = reportExporter;
    }

    async Task<PipelineResult> IPipelineService.RunAsync(PipelineContext context)
    {
        var userAnswersFormat = formatDetector.DetectFormat(context.UserAnswersFileName, context.UserAnswersStream);
        var userAnswersParser = parserFactory.GetParser(userAnswersFormat, ParsingTarget.Survey);

        var surveyParseResultList = await userAnswersParser.ParseAsync<List<SurveyParseResult>>(context.UserAnswersStream, context.UserAnswersFileName);

        var validationResult = dataValidator.Validate(surveyParseResultList);

        var surveyStatistics = new List<SurveyStatistics>();

        foreach (var validatedResult in validationResult.ValidatedResults)
        {
            surveyStatistics.Add(statisticsCalculator.Calculate(validatedResult));
        }

        //var batches = batchBuilder.Build(validationResult);

        //var allQuestionResults = new List<QuestionAnalysisResult>();

        //foreach (var batch in batches)
        //{
        //    var needAnalysis = batch.Answers
        //        .Where(a => a.PreStatus == AnswerPreStatus.NeedAnalysis)
        //        .ToList();

        //    var aiResults = new List<ComparisonResult>();
        //    if (needAnalysis.Count != 0)
        //    {
        //        var agentResponse = await testAnalysisAgent.AnalyzeBatchAsync(batch, context.AnalysisMethod == AnalysisMethod.RussianAiAgent
        //            ? LlmVariant.Russian
        //            : LlmVariant.Foreign);

        //        aiResults = agentResponse.Results.Select(r => new ComparisonResult
        //        {
        //            UserId = r.UserId,
        //            SimilarityPercent = r.SimilarityPercent,
        //            Verdict = r.Verdict,
        //            Comment = r.Comment
        //        }).ToList();
        //    }

        //    var finalResults = batch.Answers.Select(item =>
        //    {
        //        if (item.PreStatus == AnswerPreStatus.ExactMatch)
        //        {
        //            return new ComparisonResult { UserId = item.UserId, SimilarityPercent = 100, Verdict = "correct" };
        //        }
        //        else if (item.PreStatus == AnswerPreStatus.Empty)
        //        {
        //            return new ComparisonResult { UserId = item.UserId, SimilarityPercent = 0, Verdict = "incorrect", Comment = "пустой ответ" };
        //        }
        //        return aiResults.FirstOrDefault(r => r.UserId == item.UserId) ?? new ComparisonResult { UserId = item.UserId, SimilarityPercent = 50, Verdict = "partial" };
        //    }).ToList();

        //    allQuestionResults.Add(new QuestionAnalysisResult
        //    {
        //        Question = batch.Question,
        //        Results = finalResults
        //    });
        //}

        //var reportData = await reportBuilder.BuildAsync(allQuestionResults, context.AnalysisMethod);
        //var excelBytes = reportExporter.ExportToExcel(reportData);

        //reportData.ParsedUsers = parsedUserAnswers;

        return new PipelineResult()
        {
            ValidationResult = validationResult,
            SurveyStatistics = surveyStatistics,
            //ReportData = reportData,
            //ExcelReport = excelBytes,
            //Errors = validationResult.Warnings.ToList(),
        };
    }

    async Task<byte[]> IPipelineService.ExportReportExcel(ReportData reportData)
    {
        //var excelBytes = reportExporter.ExportToExcel(reportData);
        //return excelBytes;
        return [];
    }
}
