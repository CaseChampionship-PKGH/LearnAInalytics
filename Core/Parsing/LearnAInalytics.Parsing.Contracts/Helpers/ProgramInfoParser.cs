using System.Text.RegularExpressions;
using LearnAInalytics.Entities.Models;

namespace LearnAInalytics.Parsing.Contracts.Helpers;

/// <summary>
/// Парсер программы
/// </summary>
public static class ProgramInfoParser
{
    /// <summary>
    /// Распарсить программу
    /// </summary>
    public static ProgramInfo Parse(string fileName)
    {
        var info = new ProgramInfo();

        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

        var match = Regex.Match(nameWithoutExt, @"^[\d\.\-\s]+(?=[А-ЯЁA-Z]|[а-яёa-z])");
        if (match.Success)
        {
            info.Period = match.Value.Trim();
            info.Title = nameWithoutExt.Substring(match.Index + match.Length).Trim();
        }
        else
        {
            info.Title = nameWithoutExt;
        }

        return info;
    }
}
