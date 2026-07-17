using AutoMapper;
using LearnAInalytics.Analysis.Contracts.Models;
using LearnAInalytics.Api.Models;

namespace LearnAInalytics.Api.AutoMappers;

/// <inheritdoc />
public class SurveyAnalysisApiProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SurveyAnalysisApiProfile"/>
    /// </summary>
    public SurveyAnalysisApiProfile()
    {
        CreateMap<AnalysisResult, AnalysisResultApiModel>(MemberList.Source)
            .ForMember(x => x.Errors, opt => opt.Ignore())
            .ReverseMap();
    }
}
