using System.Text;

namespace NCICParser.src;

/// <summary>
/// Handles Reading in the current NCIC Code file
/// </summary>
internal static class NCICFileReader
{
    /// <summary>
    /// Reads the NCIC Code File
    /// </summary>
    /// <param name="filePath">Fully Qualified Path to the NCIC Text file</param>
    /// <returns></returns>
    public static List<RawLine> ReadFile(string filePath)
    {
        List<RawLine> results = [];

        int pageNumber = 1;
        int pageLineNumber = 0;
        int lineNumber = 0;

        foreach(string line in File.ReadAllLines(filePath))
        {
            if (line.StartsWith('\f'))
            {
                pageNumber++;
                pageLineNumber = 0;
            }

            results.Add(new RawLine(line, ++lineNumber, pageNumber, ++pageLineNumber));
        }

        return results;
    }
}