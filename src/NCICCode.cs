namespace NCICParser.src;

/// <summary>
/// Defines the common output structure to Code csv file.
/// </summary>
/// <param name="line">Current RawLine</param>
internal class NCICCode(
    RawLine line)
        : RawLine(line.Text, line.FileLineNumber, line.PageNumber, line.PageLineNumber)
{
    /// <summary>
    /// Gets or Sets the CodeSet (if determined)
    /// </summary>
    public string CodeSet { get; set; } = "";

    /// <summary>
    /// Gets or Sets the Group Name
    /// </summary>
    public string GroupName { get; set; } = "";

    /// <summary>
    /// Gets or Sets the Section Name
    /// </summary>
    public string SectionName { get; set; } = "";

    /// <summary>
    /// Gets or Sets the ParentCode (if any)
    /// </summary>
    public string ParentCode { get; set; } = "";

    /// <summary>
    /// Gets or Sets the ParentDescription (if any)
    /// </summary>
    public string ParentDescription { get; set; } = "";

    /// <summary>
    /// Gets or sets the Ordinal Code ColumnNumber
    /// </summary>
    public int ColumnNumber { get; set; }

    /// <summary>
    /// Gets or Sets the Code ColumnName
    /// </summary>
    public string ColumnName { get; set; } = "";

    /// <summary>
    /// Gets or Sets the Code Column Value (if any)
    /// </summary>
    public string Value { get; set; } = "";
}
