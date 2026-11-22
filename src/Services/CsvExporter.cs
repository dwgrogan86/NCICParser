using NCICParser.Interfaces;
using NCICParser.Models;
using System.Text;

namespace NCICParser.Services;

/// <summary>
/// Handles exporting the parsed content to a csv file
/// </summary>
public class CsvExporter
    : ICsvExporter
{
    /// <summary>
    /// Maps the parsed content to a common code or reject structure and outputs to csv files
    /// </summary>
    /// <param name="sections"></param>
    public void GenerateFiles(List<Section> sections, string outputFolder)
    {
        List<CodeExportRow> goodItems = [];
        List<ParseReject> badItems = [];

        foreach (var section in sections)
        {
            foreach (var subsection in section.SubSections)
            {
                var rows = subsection.CodeItemRows.OrderBy(i => i.SourceLine.FileLineNumber).ToList();
                for (var idx = 0; idx < rows.Count; idx++)
                {
                    var row = rows[idx];

                    if (row.Rejected)
                    {
                        ParseReject code = new(row.SourceLine, section.Name);
                        badItems.Add(code);
                    }
                    else
                    {
                        for (var jdx = 0; jdx < row.CodeItemValues.Count; jdx++)
                        {
                            CodeExportRow code = new(row.SourceLine)
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

        var folderExists = CreateFolderIfRequired(outputFolder);

        if (folderExists)
        {
            WriteCodes(Path.Combine(outputFolder, "NCICCodes.csv"), goodItems);
            WriteRejects(Path.Combine(outputFolder, "NCICRejects.csv"), badItems);
        }
    }

    /// <summary>
    /// Creates the output folder if it does not exist
    /// </summary>
    /// <param name="outputFolder"></param>
    /// <returns></returns>
    private static bool CreateFolderIfRequired(string outputFolder)
    {
        bool returnValue = Directory.Exists(outputFolder);

        if (!returnValue)
        {
            try
            {
                var info = Directory.CreateDirectory(outputFolder);
                returnValue = info.Exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating output folder: \"{outputFolder}\"\n, {ex.Message}");
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Writes out the 'good' codes to file.
    /// </summary>
    /// <param name="fileName">Output file name</param>
    /// <param name="codes">List of NCICCodes</param>
    private static void WriteCodes(
        string fileName,
        IEnumerable<CodeExportRow> codes)
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
                    code.SourceLine.PageNumber,
                    code.SourceLine.PageLineNumber,
                    code.SourceLine.FileLineNumber
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
                    reject.SourceLine.PageNumber,
                    reject.SourceLine.PageLineNumber,
                    reject.SourceLine.FileLineNumber,
                    Escape(reject.SourceLine.Text)
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