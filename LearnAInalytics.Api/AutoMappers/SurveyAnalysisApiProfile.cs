using AutoMapper;
using LearnAInalytics.Api.Models;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Api.AutoMappers;

/// <inheritdoc />
public class SurveyAnalysisApiProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAnalysisApiProfile"/>
    /// </summary>
    public SurveyAnalysisApiProfile()
    {
        CreateMap<ReportDataApiModel, ReportData>(MemberList.Source).ReverseMap();
        CreateMap<SummaryApiModel, Summary>(MemberList.Source).ReverseMap();
        CreateMap<QuestionReportApiModel, QuestionReport>(MemberList.Source).ReverseMap();
    }
}
