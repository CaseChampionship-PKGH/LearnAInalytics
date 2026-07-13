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
        CreateMap<AnalysisResultApiModel, AnalysisResult>(MemberList.Source).ReverseMap();
    }
}
