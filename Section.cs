using System.Text;
using System.Text.RegularExpressions;

namespace NCICParser;

/// <summary>
/// Represents a Code Section
/// </summary>
/// <param name="line">Starting RawLine</param>
/// <param name="name">Section Name</param>
internal class Section(
    RawLine line,
    string name) 
        : NamedTrackedItem(line, name)
{
    /// <summary>
    /// Gets or Sets the FileLineNumberEnd for this section
    /// </summary>
    public long FileLineNumberEnd { get; set; } = -1;

    /// <summary>
    /// Gets or Sets the CodeColumns
    /// </summary>
    public List<CodeColumn> CodeColumns { get; set; } = [];

    /// <summary>
    /// Gets or Sets the CodeItemRows
    /// </summary>
    public List<CodeItemRow> CodeItemRows { get; set; } = [];

    /// <summary>
    /// Parses out the Section Code from the Section Name (when available)
    /// </summary>
    /// <returns></returns>
    public string SectionCode()
    {
        const string pattern = @"(?<=\()[^)]*(?=\))";

        StringBuilder returnValue = new();
        var matches = Regex.Matches(Name, pattern, RegexOptions.Compiled | RegexOptions.Singleline);
        foreach (Match match in matches)
        {
            if (returnValue.Length > 0)
            {
                returnValue.Append('/');
            }

            var tempItems = match.Value.Split(",", StringSplitOptions.TrimEntries);
            var tempConcat = string.Join("/", tempItems);
            returnValue.Append(tempConcat);
        }

        return returnValue.ToString();
    }

    /// <summary>
    /// Gets the Code List Regex Pattern to parse the values.
    /// </summary>
    /// <returns></returns>
    public string GetCodeRegExPattern()
    {
        StringBuilder returnValue = new();
        returnValue.Append('^');

        for (int i = 0; i < CodeColumns.Count; i++)
        {
            returnValue.Append("(.{1,");

            if (i < CodeColumns.Count - 1)
                returnValue.Append(CodeColumns[i].ColumnWidth);

            returnValue.Append("})");
        }

        return returnValue.ToString();
    }
}
