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
    /// Gets or Sets the Section Name
    /// </summary>
    public string SectionName { get; set; } = "";

    /// <summary>
    /// Gets or Sets the SubSection Name
    /// </summary>
    public string SubSectionName { get; set; } = "";

    /// <summary>
    /// Gets or Sets the CategoryCode (if any)
    /// </summary>
    public string CategoryCode { get; set; } = "";

    /// <summary>
    /// Gets or Sets the CategoryDescription (if any)
    /// </summary>
    public string CategoryDescription { get; set; } = "";

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
