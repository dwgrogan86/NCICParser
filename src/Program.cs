namespace NCICParser.src;

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
        string filePathText = @".\Resources\nciccodemanual.txt";

        // Parse the file returning all groups
        var groups = Parser.Parse(filePathText);

        // Export parsed content to csv files.
        CsvExporter.GenerateFiles(groups);
    }
}
