using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Interfaces;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Entities.Models;
using LearnAInalytics.Parsing.Contracts.Enums;
using LearnAInalytics.Parsing.Contracts.Interfaces;
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
    private readonly ICriterionAggregator criterionAggregator;
    private readonly ISurveyCriteriaAnalysisAgent surveyCriteriaAnalysisAgent;
    private readonly IReportExporterFactory reportExporterFactory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAnalysisPipeline"/>
    /// </summary>
    public SurveyAnalysisPipeline(IFormatDetector formatDetector,
        IParserFactory parserFactory,
        IDataValidator dataValidator,
        IStatisticsCalculator statisticsCalculator,
        ICriterionAggregator criterionAggregator,
        ISurveyCriteriaAnalysisAgent surveyCriteriaAnalysisAgent,
        IReportExporterFactory reportExporterFactory)
    {
        this.formatDetector = formatDetector;
        this.parserFactory = parserFactory;
        this.dataValidator = dataValidator;
        this.statisticsCalculator = statisticsCalculator;
        this.criterionAggregator = criterionAggregator;
        this.surveyCriteriaAnalysisAgent = surveyCriteriaAnalysisAgent;
        this.reportExporterFactory = reportExporterFactory;
    }

    async Task<PipelineResult> IPipelineService.RunAsync(PipelineContext context)
    {
        var userAnswersFormat = formatDetector.DetectFormat(context.UserAnswersFileName, context.UserAnswersStream);
        var userAnswersParser = parserFactory.GetParser(userAnswersFormat, ParsingTarget.Survey);

        var surveyParseResult = await userAnswersParser.ParseAsync<Survey>(context.UserAnswersStream, context.UserAnswersFileName);

        var validationResult = dataValidator.Validate(surveyParseResult);

        var statistics = statisticsCalculator.Calculate(validationResult.ValidatedResults);
        var aggregationResult = criterionAggregator.Aggregate(validationResult.ValidatedResults, statistics);

        var criterionAnalysisList = new List<CriterionAnalysis>();

        foreach (var promptData in aggregationResult.AllCriteriaData)
        {
            //var note = await surveyCriteriaAnalysisAgent.AnalyzeCriterionAsync(promptData, context.AnalysisMethod == AnalysisMethod.RussianAiAgent
            //        ? LlmVariant.Russian
            //        : LlmVariant.Foreign);

            var note = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In quis eros varius, viverra neque et, pretium ligula. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas id blandit sapien. Mauris eu mauris urna. Fusce auctor tellus maximus viverra cursus. In aliquam et augue non fringilla. Vestibulum non congue ex. Proin feugiat leo quis nisl scelerisque pulvinar. Nam consequat quam nec urna congue, sed tristique erat molestie. Pellentesque pharetra neque sit amet tellus vestibulum, a hendrerit nisi fermentum. In lobortis laoreet ornare. Suspendisse eros libero, placerat vitae nulla in, rhoncus blandit nibh. Suspendisse tincidunt pretium enim, efficitur dictum leo iaculis vel. Nam eget ante nibh. Etiam mauris erat, aliquet vel mauris ac, ullamcorper imperdiet massa. Duis dolor nisl, ultricies vitae lorem id, eleifend egestas sem.\r\n\r\nNulla facilisi. Praesent eget ultricies enim. Ut non lobortis massa, vitae imperdiet dui. Fusce sed bibendum dolor. Morbi ac orci in est consectetur placerat nec ac dui. Aenean ultrices mi sit amet sapien vehicula, eu semper nisl iaculis. Morbi eu risus quis nisl sodales euismod at in libero. Cras ut malesuada libero. Aliquam erat volutpat. Donec ornare placerat metus, sollicitudin semper mi imperdiet et. Nam ac risus sit amet justo tempor eleifend. Cras eget varius felis.\r\n\r\nDonec aliquet ex non felis consequat, ut facilisis lorem pulvinar. Nunc at nisl vitae elit tincidunt faucibus. Duis arcu ex, vestibulum et erat quis, sollicitudin luctus magna. Aliquam fermentum tincidunt risus, eget vestibulum neque euismod in. Nam vitae eros id elit lacinia ultrices. Curabitur ut blandit tellus. Sed elementum feugiat sem. Integer dui tellus, cursus quis efficitur eget, faucibus eu sem. Fusce et suscipit risus. Praesent sollicitudin vel ex et dictum. Nunc vitae volutpat sapien. Fusce vestibulum arcu diam, ac vehicula enim luctus in. Suspendisse potenti. Donec ut eros quam.\r\n\r\n";

            criterionAnalysisList.Add(new CriterionAnalysis
            {
                CriterionData = promptData,
                Note = note
            });
        }

        //var trajectory = await surveyCriteriaAnalysisAgent.AnalyzeTrajectoryAsync(aggregationResult,
        //    criterionAnalysisList.Select(x => x.Note ?? string.Empty).ToList(),
        //    context.AnalysisMethod == AnalysisMethod.RussianAiAgent
        //            ? LlmVariant.Russian
        //            : LlmVariant.Foreign);

        var trajectory = new Trajectory()
        {
            SuggestedTopicsSummary = "Nulla facilisi. Praesent eget ultricies enim. Ut non lobortis massa, vitae imperdiet dui. Fusce sed bibendum dolor. Morbi ac orci in est consectetur placerat nec ac dui. Aenean ultrices mi sit amet sapien vehicula, eu semper nisl iaculis. Morbi eu risus quis nisl sodales euismod at in libero. Cras ut malesuada libero. Aliquam erat volutpat. Donec ornare placerat metus, sollicitudin semper mi imperdiet et. Nam ac risus sit amet justo tempor eleifend. Cras eget varius felis.\r\n\r\n",
            ExcludedTopicsSummary = "Vivamus posuere quam neque. Ut diam eros, luctus eu ligula eu, pellentesque convallis libero. Mauris dignissim ex arcu, nec gravida velit pellentesque id. Nunc a magna sit amet nunc venenatis viverra id in dui. Fusce vitae efficitur felis, et eleifend purus. Duis et odio odio. Duis ac magna consequat, tristique risus eu, tincidunt urna. ",
            ProgramSupplement = "Pellentesque scelerisque neque nulla, nec volutpat justo vehicula mattis. Vivamus porttitor purus bibendum odio maximus, id facilisis mi suscipit. Etiam ultrices convallis metus, id iaculis magna scelerisque eget. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent ac sapien ligula. Sed ut porttitor sem. ",
            NeedForProgram = "Нет необходимости",
            HoursChange = "Нет необходимости",
            AdmissionCorrection = "Нет необходимости",
            FormChange = "Нет необходимости",
        };

        var result = new AnalysisResult()
        {
            ProgramInfo = surveyParseResult.ProgramInfo,
            AllCriteriaAnalysisData = criterionAnalysisList,
            FormatDistribution = aggregationResult.FormatDistribution,
            Trajectory = trajectory
        };

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
            AnalysisResult = result,
            Errors = validationResult.Warnings.ToList(),
            //ReportData = reportData,
            //ExcelReport = excelBytes,
        };
    }

    async Task<byte[]> IPipelineService.ExportStatsExcel(AnalysisResult result)
    {
        var reportExporter = reportExporterFactory.GetReportExporter(ExportType.Excel);
        var excelBytes = reportExporter.Export(result);
        return excelBytes;
    }


    async Task<byte[]> IPipelineService.ExportReportWord(AnalysisResult result)
    {
        var reportExporter = reportExporterFactory.GetReportExporter(ExportType.Word);
        var excelBytes = reportExporter.Export(result);
        return excelBytes;
    }
}
