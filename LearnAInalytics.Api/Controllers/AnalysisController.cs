using AutoMapper;
using LearnAInalytics.Analysis.Contracts.Enums;
using LearnAInalytics.Api.Models;
using LearnAInalytics.Parsing.Contracts.Exceptions;
using LearnAInalytics.Reporting.Contracts.Models;
using LearnAInalytics.Services.Contracts.Interfaces;
using LearnAInalytics.Services.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAInalytics.Api.Controllers;

/// <summary>
/// Управление анализом тестов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IPipelineService pipeline;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AnalysisController"/>
    /// </summary>
    public AnalysisController(IPipelineService pipeline, IMapper mapper)
    {
        this.pipeline = pipeline;
        this.mapper = mapper;
    }

    /// <summary>
    /// Запуск анализа и получение структурированного отчёта (JSON).
    /// </summary>
    [HttpPost("run")]
    public async Task<IActionResult> RunAnalysis(
        IFormFile userAnswers,
        [FromQuery] AnalysisMethod analysisMethod = AnalysisMethod.RussianAiAgent)
    {
        if (userAnswers == null || userAnswers.Length == 0)
        {
            return BadRequest("Файл с ответами тестируемых обязателен.");
        }

        var context = new PipelineContext
        {
            UserAnswersStream = userAnswers.OpenReadStream(),
            UserAnswersFileName = userAnswers.FileName,
            AnalysisMethod = analysisMethod
        };

        try
        {
            var result = await pipeline.RunAsync(context);
            //var mappedResult = mapper.Map<ReportDataApiModel>(result.ParsedResult);
            return Ok(result.ParsedResult);
        }
        catch (ParsingException ex)
        {
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Внутренняя ошибка сервера.", detail = ex.Message });
        }
    }

    /// <summary>
    /// Экспорт отчёта в Pdf (.pdf)
    /// </summary>
    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportToPDF([FromBody] ReportDataApiModel reportModel)
    {
        var domainReport = mapper.Map<ReportData>(reportModel);
        var excelBytes = await pipeline.ExportReportExcel(domainReport);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }

    /// <summary>
    /// Проверка работоспособности сервиса.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
}
