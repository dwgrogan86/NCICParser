using NCICParser.Interfaces;
using NCICParser.Models;

namespace NCICParser.Services;

/// <summary>
/// Handles Reading in the current NCIC Code file
/// </summary>
public class NCICFileReader
    : INCICFileReader
{
    /// <summary>
    /// Reads the NCIC Code File
    /// </summary>
    /// <param name="filePath">Fully Qualified Path to the NCIC Text file</param>
    /// <returns>List of RawLine objects</returns>
    public List<RawLine> ReadFile(string filePath)
    {
        List<RawLine> returnValue = [];

        int pageNumber = 1;
        int pageLineNumber = 0;
        int lineNumber = 0;

        foreach (string line in File.ReadAllLines(filePath))
        {
            if (line.StartsWith('\f'))
            {
                pageNumber++;
                pageLineNumber = 0;
            }

            returnValue.Add(new RawLine(line, ++lineNumber, pageNumber, ++pageLineNumber));
        }

        return returnValue;
    }
}