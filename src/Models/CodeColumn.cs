namespace NCICParser.Models;

/// <summary>
/// Represents a single code column
/// </summary>
public class CodeColumn (
    string columnName,
    int columnStart)
{
    /// <summary>
    /// Gets or Sets the ColumnName
    /// </summary>
    public string ColumnName { get; init; } = columnName;

    /// <summary>
    /// Gets or Sets the ColumnStart
    /// </summary>
    public int ColumnStart { get; init; } = columnStart;

    /// <summary>
    /// Gets or Sets the ColumnWidth
    /// </summary>
    public int ColumnWidth { get; set; }
}
