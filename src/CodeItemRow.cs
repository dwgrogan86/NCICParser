namespace NCICParser.src;

/// <summary>
/// Represents a parsed code item row and its associated source line metadata.
/// </summary>
/// <param name="line">The raw source line used to initialize the code item row.</param>
internal class CodeItemRow(
    RawLine line) 
        : RawLine(line.Text, line.FileLineNumber, line.PageNumber, line.PageLineNumber)
{
    /// <summary>
    /// Gets or set the raw category value
    /// </summary>
    public string CategoryRaw { get; set; } = string.Empty;

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
    /// Returns the Category code value from the raw value
    /// </summary>
    public string CategoryCode =>
        CategoryRaw.Length > 0
            ? CategoryRaw.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty
            : string.Empty;

    /// <summary>
    /// Returns the Category code description (if any) or the category code as the description.
    /// </summary>
    public string CategoryDescription =>
        CategoryRaw.Length > 0
            ? CategoryRaw.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty
            : string.Empty;

}
