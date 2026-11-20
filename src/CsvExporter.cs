using System.Text;

namespace NCICParser.src;

/// <summary>
/// Handles exporting the parsed content to a csv file
/// </summary>
internal static class CsvExporter
{
    /// <summary>
    /// Maps the parsed content to a common code or reject structure and outputs to csv files
    /// </summary>
    /// <param name="sections"></param>
    public static void GenerateFiles(List<Section> sections)
    {
        List<NCICCode> goodItems = [];
        List<ParseReject> badItems = [];

        foreach (var section in sections)
        {
            foreach (var subsection in section.SubSections)
            {
                var rows = subsection.CodeItemRows.OrderBy(i => i.FileLineNumber).ToList();
                for (var idx = 0; idx < rows.Count; idx++)
                {
                    var row = rows[idx];

                    if (row.Rejected)
                    {
                        ParseReject code = new(row, section.Name);
                        badItems.Add(code);
                    }
                    else
                    {
                        for (var jdx = 0; jdx < row.CodeItemValues.Count; jdx++)
                        {
                            NCICCode code = new(row)
                            {
                                ColumnNumber = jdx + 1,
                                ColumnName = subsection.CodeColumns[jdx].ColumnName,
                                Value = row.CodeItemValues[jdx],
                                CodeSet = subsection.SubSectionCode(),
                                CategoryCode = row.CategoryCode,
                                CategoryDescription = row.CategoryDescription,
                                SectionName = section.Name,
                                SubSectionName = subsection.Name
                            };

                            goodItems.Add(code);
                        }
                    }
                }
            }
        }

        WriteCodes("NCICCodes.csv", goodItems);
        WriteRejects("NCICRejects.csv", badItems);
    }

    /// <summary>
    /// Writes out the 'good' codes to file.
    /// </summary>
    /// <param name="fileName">Output file name</param>
    /// <param name="codes">List of NCICCodes</param>
    private static void WriteCodes(
        string fileName,
        IEnumerable<NCICCode> codes)
    {
        using StreamWriter writer = new(fileName, false, Encoding.UTF8);

        // Write the header
        writer.WriteLine(
            "CodeSet," +
            "SectionName," +
            "SubSectionName," +
            "CategoryCode," +
            "CategoryDescription," +
            "Column," +
            "Name," +
            "Value," +
            "PageNumber," +
            "PageLineNumber," +
            "FileLineNumber");

        // Write the content
        foreach (var code in codes)
        {
            writer.WriteLine(
                string.Join(",",
                    Escape(code.CodeSet),
                    Escape(code.SectionName),
                    Escape(code.SubSectionName),
                    Escape(code.CategoryCode),
                    Escape(code.CategoryDescription),
                    Escape(code.ColumnNumber.ToString()),
                    Escape(code.ColumnName),
                    Escape(code.Value),
                    code.PageNumber,
                    code.PageLineNumber,
                    code.FileLineNumber
                ));
        }
    }

    /// <summary>
    /// Writes out the 'bad' codes to file.
    /// </summary>
    /// <param name="fileName">Output file name</param>
    /// <param name="rejects">List of ParseRejects</param>
    private static void WriteRejects(
        string fileName,
        IEnumerable<ParseReject> rejects)
    {
        using StreamWriter writer =
            new(fileName, false, Encoding.UTF8);

        // Write the header
        writer.WriteLine(
            "Reason," +
            "SectionName," +
            "PageNumber," +
            "PageLineNumber," +
            "FileLineNumber," +
            "Text");

        // Write the content
        foreach (var reject in rejects)
        {
            writer.WriteLine(
                string.Join(",",
                    Escape(reject.Reason),
                    Escape(reject.Name),
                    reject.PageNumber,
                    reject.PageLineNumber,
                    reject.FileLineNumber,
                    Escape(reject.Text)
                ));
        }
    }

    /// <summary>
    /// Handles quoting and formatting of any escaped values to prevent literal escape value output.
    /// </summary>
    /// <param name="value">Current value</param>
    /// <returns>Quoted and escape suppressed text</returns>
    private static string Escape(string? value)
    {
        value ??= string.Empty;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}