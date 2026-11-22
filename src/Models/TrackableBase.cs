
namespace NCICParser.Models;

public abstract class TrackableBase(
    RawLine line)
{
    /// <summary>
    /// Gets or Sets the Source line
    /// </summary>
    public RawLine SourceLine { get; init; } = line ??
        throw new ArgumentException("RawLine (line) argument cannot be null");

}
