using AutoMapper;
using LearnAInalytics.Agent;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Agent.OpenAI;
using LearnAInalytics.Agent.YandexGPT;
using LearnAInalytics.Analysis;
using LearnAInalytics.Api.AutoMappers;
using LearnAInalytics.Common.Mvc.Extensions;
using LearnAInalytics.Parsing;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Csv;
using LearnAInalytics.Parsing.Excel;
using LearnAInalytics.Parsing.Json;
using LearnAInalytics.Reporting;
using LearnAInalytics.Reporting.Contracts.Interfaces;
using LearnAInalytics.Services;
using LearnAInalytics.Validation;
using Microsoft.Extensions.Logging.Abstractions;
using Module = LearnAInalytics.Common.Mvc.Module;

namespace LearnAInalytics.Api.DI;

/// <inheritdoc />
public class ApiModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyCsvParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyExcelParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, AgentResponseJsonParser>(ServiceLifetime.Singleton);

        services.AddHttpClient("YandexGPT", client =>
        {
            client.BaseAddress = new Uri("https://llm.api.cloud.yandex.net/");
        });
        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, YandexGPTllmClient>(ServiceLifetime.Singleton);

        services.AddHttpClient("OpenAI", client =>
        {
            client.BaseAddress = new Uri("https://openrouter.ai/api/v1/");
        });
        services.RegisterMultipleInterfacesAssignableTo<ILlmClient, OpenAiCompatibleLlmClient>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ParserFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<FormatDetector>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<StatisticsCalculator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<SurveyAnalysisPipeline>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DataValidator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<SurveyAggregator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DefaultPromptProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmSurveyAnalysisAgent>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IReportExporter, ExcelReportExporter>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IReportExporter, WordReportExporter>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ReportExporterFactory>(ServiceLifetime.Singleton);
        services.RegisterAutoMapperProfile<SurveyAnalysisApiProfile>();
        RegisterAutoMapper(services);
        services.AddHttpContextAccessor();
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var profiles = provider.GetServices<Profile>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            }, NullLoggerFactory.Instance);
            var mapper = mapperConfig.CreateMapper();
            return mapper;
        });
    }
}
