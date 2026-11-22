using NCICParser.Models;

namespace NCICParser.Interfaces;

/// <summary>
/// Handles Reading in the current NCIC Code file
/// </summary>
public interface INCICFileReader
{
    /// <summary>
    /// Reads the NCIC Code File
    /// </summary>
    /// <param name="filePath">Fully Qualified Path to the NCIC Text file</param>
    /// <returns>List of RawLine objects</returns>
    List<RawLine> ReadFile(string filePath);
}