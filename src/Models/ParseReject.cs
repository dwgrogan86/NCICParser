namespace NCICParser.Models;

/// <summary>
/// Represents a rejected NCIC record parsing operation and its associated rejection details.
/// </summary>
/// <param name="line">The raw source line associated with the rejected parse.</param>
/// <param name="name">The name identifying the rejected parse item.</param>
public class ParseReject(
    RawLine line,
    string name)
        : TrackableBase(line)
{
    public string Name { get; init; } = name;

    /// <summary>
    /// Gets the Reject reason.
    /// </summary>
    public string Reason { get; init; } = "Column Map No Matched";
}