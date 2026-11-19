namespace NCICParser;

/// <summary>
/// Represents a parsed code item row and its associated source line metadata.
/// </summary>
/// <param name="line">The raw source line used to initialize the code item row.</param>
internal class CodeItemRow(
    RawLine line) 
        : RawLine(line.Text, line.FileLineNumber, line.PageNumber, line.PageLineNumber)
{
    /// <summary>
    /// Gets or set the raw ParentCode value
    /// </summary>
    public string ParentCodeRaw { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the list of code item values
    /// </summary>
    public List<string> CodeItemValues { get; set; } = [];

    /// <summary>
    /// Determines if the list of code items is either zero count or all empty
    /// </summary>
    public bool CodeItemsEmpty =>
        CodeItemValues.Count == 0 || CodeItemValues.All(string.IsNullOrWhiteSpace);

    /// <summary>
    /// Determines if the row parsing was rejected (did not match the pattern)
    /// </summary>
    public bool Rejected { get; set; } = false;

    /// <summary>
    /// Returns the Parent code value from the raw value
    /// </summary>
    public string ParentCode =>
        ParentCodeRaw.Length > 0
            ? ParentCodeRaw.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty
            : string.Empty;

    /// <summary>
    /// Returns the Parent code description (if any) or the parent code as the description.
    /// </summary>
    public string ParentCodeDesc =>
        ParentCodeRaw.Length > 0
            ? ParentCodeRaw.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty
            : string.Empty;

}
