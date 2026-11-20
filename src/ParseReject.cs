namespace NCICParser.src;

/// <summary>
/// Represents a rejected NCIC record parsing operation and its associated rejection details.
/// </summary>
/// <param name="line">The raw source line associated with the rejected parse.</param>
/// <param name="name">The name identifying the rejected parse item.</param>
internal class ParseReject(
    RawLine line,
    string name): NamedTrackedItem(line, name)
{
    /// <summary>
    /// Gets or Sets the Reject reason.
    /// </summary>
    public string Reason { get; set; } = "Column Map No Matched";
}