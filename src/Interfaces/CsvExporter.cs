using NCICParser.Models;

namespace NCICParser.Interfaces;

/// <summary>
/// Handles exporting the parsed content to a csv file
/// </summary>
public interface ICsvExporter
{
    /// <summary>
    /// Maps the parsed content to a common code or reject structure and outputs to csv files
    /// </summary>
    /// <param name="sections"></param>
    void GenerateFiles(List<Section> sections, string outputFolder);
}