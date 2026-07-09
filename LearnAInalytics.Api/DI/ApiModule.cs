using AutoMapper;
using LearnAInalytics.Agent;
using LearnAInalytics.Agent.Contracts.Interfaces;
using LearnAInalytics.Agent.OpenAI;
using LearnAInalytics.Agent.YandexGPT;
using LearnAInalytics.Analysis;
using LearnAInalytics.Api.AutoMappers;
using LearnAInalytics.Common.Mvc.Extensions;
using LearnAInalytics.Parsing;
using LearnAInalytics.Parsing.Archive;
using LearnAInalytics.Parsing.Contracts.Interfaces;
using LearnAInalytics.Parsing.Csv;
using LearnAInalytics.Parsing.Excel;
using LearnAInalytics.Parsing.Json;
using LearnAInalytics.Reporting;
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
        services.AddSingleton<SurveyCsvParser>();
        services.AddSingleton<SurveyExcelParser>();
        services.AddSingleton<SurveyArchiveParser>();
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyCsvParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyExcelParser>(ServiceLifetime.Singleton);
        services.RegisterMultipleInterfacesAssignableTo<IDataParser, SurveyArchiveParser>(ServiceLifetime.Singleton);
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
        services.RegisterAsImplementedInterfaces<ExcelReportExporter>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<SurveyAnalysisPipeline>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DataValidator>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<QuestionBatchBuilder>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<DefaultPromptProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmFactory>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmTestAnalysisAgent>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<LlmReportAgent>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<AgentReportBuilder>(ServiceLifetime.Singleton);
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
