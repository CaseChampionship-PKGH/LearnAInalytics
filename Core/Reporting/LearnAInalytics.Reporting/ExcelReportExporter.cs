using ClosedXML.Excel;
using LearnAInalytics.Reporting.Contracts.Interfaces;
using LearnAInalytics.Reporting.Contracts.Models;

namespace LearnAInalytics.Reporting;

/// <inheritdoc cref="IReportExporter"/>
public class ExcelReportExporter : IReportExporter
{
    byte[] IReportExporter.ExportToExcel(ReportData report)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Анализ тестов");

        var titleCell = ws.Cell(1, 1);
        titleCell.Value = "Отчёт анализа тестовых заданий";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 6).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Row(1).Height = 30;

        ws.Cell(3, 1).Value = "Сводная статистика";
        ws.Cell(3, 1).Style.Font.Bold = true;
        ws.Cell(4, 1).Value = "Всего вопросов";
        ws.Cell(4, 2).Value = report.Summary.TotalQuestions;
        ws.Cell(5, 1).Value = "Всего тестируемых";
        ws.Cell(5, 2).Value = report.Summary.TotalUsers;
        ws.Cell(6, 1).Value = "Ср. % правильных";
        ws.Cell(6, 2).Value = report.Summary.OverallCorrectPercentage / 100;
        ws.Cell(6, 2).Style.NumberFormat.Format = "0.00%";

        var startRow = 8;
        ws.Cell(startRow, 1).Value = "Детализация по вопросам";
        ws.Cell(startRow, 1).Style.Font.Bold = true;
        startRow++;

        string[] headers = {
            "Вопрос", "Правильный ответ", "Тип", "Всего ответов",
            "Правильных", "Частично", "Неправильных", "% правильных",
            "Критический", "Пустых", "Ср. совпадение", "Частые ошибки"
        };

        for (var i = 0; i < headers.Length; i++)
        {
            ws.Cell(startRow, i + 1).Value = headers[i];
            ws.Cell(startRow, i + 1).Style.Font.Bold = true;
            ws.Cell(startRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        var dataStart = startRow + 1;
        foreach (var q in report.Questions)
        {
            var row = dataStart++;
            ws.Cell(row, 1).Value = q.QuestionText;
            ws.Cell(row, 2).Value = q.CorrectAnswer;
            ws.Cell(row, 3).Value = q.Type;
            ws.Cell(row, 4).Value = q.TotalAnswers;
            ws.Cell(row, 5).Value = q.CorrectCount;
            ws.Cell(row, 6).Value = q.PartialCount;
            ws.Cell(row, 7).Value = q.IncorrectCount;
            ws.Cell(row, 8).Value = q.CorrectPercentage / 100;
            ws.Cell(row, 8).Style.NumberFormat.Format = "0.00%";
            ws.Cell(row, 9).Value = q.IsCritical ? "Да" : "Нет";
            ws.Cell(row, 10).Value = q.EmptyCount;
            ws.Cell(row, 11).Value = q.AverageSimilarityPercent / 100;
            ws.Cell(row, 11).Style.NumberFormat.Format = "0.00%";
            ws.Cell(row, 12).Value = string.Join("; ", q.CommonMistakes);

            if (q.IsCritical)
            {
                ws.Row(row).Style.Fill.BackgroundColor = XLColor.LightSalmon;
            }

            if (q.EmptyCount > 0)
            {
                ws.Cell(row, 10).Style.Fill.BackgroundColor = XLColor.Yellow;
            }
        }

        var issuesRow = dataStart + 1;
        ws.Cell(issuesRow, 1).Value = "Критические замечания";
        ws.Cell(issuesRow, 1).Style.Font.Bold = true;
        issuesRow++;
        foreach (var issue in report.CriticalIssues)
        {
            ws.Cell(issuesRow++, 1).Value = issue;
            ws.Cell(issuesRow - 1, 1).Style.Fill.BackgroundColor = XLColor.OrangeRed;
        }

        var recRow = issuesRow + 1;
        ws.Cell(recRow, 1).Value = "Рекомендации";
        ws.Cell(recRow, 1).Style.Font.Bold = true;
        ws.Cell(recRow + 1, 1).Value = report.Recommendations;
        ws.Range(recRow + 1, 1, recRow + 1, 6).Merge();
        ws.Cell(recRow + 1, 1).Style.Alignment.WrapText = true;

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
