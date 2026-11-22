using System.Text.RegularExpressions;

namespace NCICParser.Models;


/// <summary>
/// Represents the raw source file line
/// </summary>
/// <param name="text">The source text</param>
/// <param name="fileLineNumber">the file line number</param>
/// <param name="pageNumber">the calculated page number</param>
/// <param name="pageLineNumber">the calculated page line number offset</param>
public partial class RawLine(string text,
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
    /// Determines if the text is null or white space
    /// </summary>
    public bool IsNullOrWhiteSpace =>
        string.IsNullOrWhiteSpace(Text);

    /// <summary>
    /// Determines if the text begins with a form-feed
    /// </summary>
    public bool IsFormFeedLine =>
        Text.StartsWith('\f');

    /// <summary>
    /// Determines if this line is a code list category line
    /// </summary>
    public bool IsCategoryLine =>
        CategoryLineRegex().IsMatch(Text);

    /// <summary>
    /// Determines if this line is a section header
    /// </summary>
    public bool IsSectionHeader =>
        SectionHeaderRegex().IsMatch(Text);

    /// <summary>
    /// Determines if this line is a code header
    /// </summary>
    public bool IsCodeHeader =>
        CodeHeaderRegex().IsMatch(Text);

    /// <summary>
    /// Determines if this line is solely a date value (used for end of page tracking)
    /// </summary>
    public bool IsDateLine =>
        DateTime.TryParse(Text, out DateTime _);

    /// <summary>
    /// Determines if this line is a "Table of Contents" line
    /// </summary>
    public bool IsTableOfContents =>
        TableOfContentsRegex().IsMatch(Text);

    /// <summary>
    /// Parses code list column specifications from the line
    /// </summary>
    /// <returns>List of CodeColumns</returns>
    /// <exception cref="Exception"></exception>
    public List<CodeColumn> GetColumnSpecs()
    {
        const int LastFieldPad = 255;

        List<CodeColumn> returnValue = [];

        if (!IsCodeHeader)
            return returnValue;

        var matches = ColumnSpecsRegex().Matches(Text);
        CodeColumn? previousColumn = null;

        foreach (Match m in matches)
        {
            CodeColumn item = new(m.Value, m.Index);

            returnValue.Add(item);
            previousColumn?.ColumnWidth = item.ColumnStart - previousColumn.ColumnStart;
            previousColumn = item;
        }

        previousColumn?.ColumnWidth = LastFieldPad;

        return returnValue;
    }

    /// <summary>
    /// Override to represent the string of this object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Line: {FileLineNumber}: \"{Text}\"";
    }

}
