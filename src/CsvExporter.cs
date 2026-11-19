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
    /// <param name="groups"></param>
    public static void GenerateFiles(List<SectionGroup> groups)
    {
        List<NCICCode> goodItems = [];
        List<ParseReject> badItems = [];

        foreach (var grp in groups)
        {
            foreach (var section in grp.Sections)
            {
                var rows = section.CodeItemRows.OrderBy(i => i.FileLineNumber).ToList();
                for (var idx = 0; idx < rows.Count; idx++)
                {
                    var row = rows[idx];

                    if (row.Rejected)
                    {
                        ParseReject code = new(row, grp.Name)
                        {
                            Reason = "Column Map No Matched",
                        };
                        badItems.Add(code);
                    }
                    else
                    {
                        for (var jdx = 0; jdx < row.CodeItemValues.Count; jdx++)
                        {
                            NCICCode code = new(row)
                            {
                                ColumnNumber = jdx + 1,
                                ColumnName = section.CodeColumns[jdx].ColumnName,
                                Value = row.CodeItemValues[jdx],
                                CodeSet = section.SectionCode(),
                                ParentCode = row.ParentCode,
                                ParentDescription = row.ParentCodeDesc,
                                GroupName = grp.Name,
                                SectionName = section.Name
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
            "ParentCode," +
            "ParentDescription," +
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
                    Escape(code.GroupName),
                    Escape(code.SectionName),
                    Escape(code.ParentCode),
                    Escape(code.ParentDescription),
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
            "LineNumber," +
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