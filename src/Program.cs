using NCICParser.Services;
using System.Diagnostics.CodeAnalysis;

namespace NCICParser;

[ExcludeFromCodeCoverage]
class Program
{
    /// <summary>
    /// Main entry point to program
    /// </summary>
    static void Main()
    {
        // Current Path to the NCIC Code text file.
        // NOTE: This should be extracted from the available national NCIC code pdf file
        //       prior to processing with this utility. Both the pdf and extracted text
        //       file are provided in this solution.
        string dateEoPString = Constants.PDF_FILE_DATE_2026;

        string filePathText = (dateEoPString == Constants.PDF_FILE_DATE_2023)
            ? @".\Resources\nciccodemanual.txt"
            : @".\Resources\NCIC-Code-Manual-June-2026.txt";

        // Create the parser, injecting the reader
        var parser = new Parser(new NCICFileReader());

        // Parse the file returning all sections
        var sections = parser.Parse(filePathText, dateEoPString);

        // Export parsed content to csv files.
        new CsvExporter().GenerateFiles(sections, AppContext.BaseDirectory);
    }
}
