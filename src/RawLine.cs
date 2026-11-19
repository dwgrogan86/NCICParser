using System.Text.RegularExpressions;

namespace NCICParser.src;

/// <summary>
/// Represents the raw source file line
/// </summary>
/// <param name="text">The source text</param>
/// <param name="fileLineNumber">the file line number</param>
/// <param name="pageNumber">the calculated page number</param>
/// <param name="pageLineNumber">the calculated page line number offset</param>
internal class RawLine(string text,
        int fileLineNumber,
        int pageNumber,
        int pageLineNumber)
{
    /// <summary>
    /// Gets or Sets the Text value
    /// </summary>
    public string Text { get; init; } = text;

    /// <summary>
    /// Gets or Sets the FileLineNumber value
    /// </summary>
    public int FileLineNumber { get; init; } = fileLineNumber;

    /// <summary>
    /// Gets or Sets the PageNumber
    /// </summary>
    public int PageNumber { get; init; } = pageNumber;

    /// <summary>
    /// Gets or Sets the PageLineNumber
    /// </summary>
    public int PageLineNumber { get; init; } = pageLineNumber;

    /// <summary>
    /// Gets the zero-based index used for array / list indexing
    /// </summary>
    public int Index =>
        FileLineNumber - 1;

    /// <summary>
    /// Determines if the text is blank
    /// </summary>
    public bool IsBlank =>
        string.IsNullOrWhiteSpace(Text);

    /// <summary>
    /// Determines if the text begins with a form-feed
    /// </summary>
    public bool IsFormFeedLine =>
        Text.StartsWith('\f');

    /// <summary>
    /// Determines if this line is a code list parent line
    /// </summary>
    public bool IsParentLine =>
        IsLineOfType(@"^[A-Za-z0-9]+\:[A-Za-z0-9 ]*");

    /// <summary>
    /// Determines if this line is a section header
    /// </summary>
    public bool IsSectionHeader =>
        IsLineOfType(@"^\f\s*\d*(\.\d*)?\s-\s.*");

    /// <summary>
    /// Determines if this line is a code header
    /// </summary>
    public bool IsCodeHeader =>
        IsLineOfType(@"^(?!.*=)(?![ 0-9])(?!\S*/)\S{3,}(?:\s+\S+)*\s{2,}\S+");

    /// <summary>
    /// Determines if this line is solely a date value (used for end of page tracking)
    /// </summary>
    public bool IsDateLine =>
        DateTime.TryParse(Text, out DateTime _);

    /// <summary>
    /// Determines if this line is a "Table of Contents" line
    /// </summary>
    public bool IsTableOfContents =>
        IsLineOfType(@"^\s*Table of Contents\s*$");

    /// <summary>
    /// Parses code list column specifications from the line
    /// </summary>
    /// <returns>List of CodeColumns</returns>
    /// <exception cref="Exception"></exception>
    public List<CodeColumn> GetColumnSpecs()
    {
        List<CodeColumn> returnValue = [];
        string pattern = @"(?<=^|\s{2,})\S.*?(?=\s{2,}|$)";

        if (!IsCodeHeader)
            return returnValue;

        var matches = Regex.Matches(Text, pattern, RegexOptions.Compiled | RegexOptions.Singleline);

        foreach(Match m in matches)
        {
            CodeColumn item = new()
            {
                ColumnName = m.Value,
                ColumnStart = m.Index
            };

            returnValue.Add(item);
        }

        // Go in reverse order to get the column widths
        for (int i = returnValue.Count - 1; i >= 0; i--)
        {
            CodeColumn col = returnValue[i];

            if (i == returnValue.Count - 1)
                col.ColumnWidth = 255;   // Last Column make sure it get's everything to the end..
            else
                col.ColumnWidth = returnValue[i + 1].ColumnStart - col.ColumnStart;

            if (col.ColumnWidth == 0)
                throw new Exception("Cannot Use Zero length Column Width");
        }

        return returnValue;
    }

    /// <summary>
    /// Override to represent the string of this object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[{FileLineNumber}] {Text}";
    }

    /// <summary>
    /// Runs the Regex pattern against this line to see if it matches
    /// </summary>
    /// <param name="pattern">Current pattern</param>
    /// <returns></returns>
    private bool IsLineOfType(string pattern) =>
        !string.IsNullOrWhiteSpace(Text) &&
        Regex.IsMatch(Text, pattern, RegexOptions.Compiled | RegexOptions.Singleline);

}
