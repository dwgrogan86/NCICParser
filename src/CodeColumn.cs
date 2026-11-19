namespace NCICParser.src;

/// <summary>
/// Represents a single code column
/// </summary>
internal class CodeColumn
{
    /// <summary>
    /// Gets or Sets the ColumnName
    /// </summary>
    public string ColumnName { get; set; } = null!;

    /// <summary>
    /// Gets or Sets the ColumnStart
    /// </summary>
    public int ColumnStart { get; set; }

    /// <summary>
    /// Gets or Sets the ColumnWidth
    /// </summary>
    public int ColumnWidth { get; set; }
}
