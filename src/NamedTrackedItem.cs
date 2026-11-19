namespace NCICParser.src;

/// <summary>
/// Represents a named item associated with a raw source line.
/// </summary>
/// <param name="line">The raw source line used to initialize the tracked item.</param>
/// <param name="name">The initial name value</param>
internal abstract class NamedTrackedItem(
    RawLine line,
    string name)
        : RawLine(line.Text, line.FileLineNumber, line.PageNumber, line.PageLineNumber)
{

    /// <summary>
    /// Gets or Sets the Name value.
    /// </summary>
    public string Name { get; set; } = name;
}
