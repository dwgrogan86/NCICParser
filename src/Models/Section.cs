namespace NCICParser.Models;

/// <summary>
/// Represents a high level Section
/// </summary>
/// <param name="line">Starting RawLine</param>
/// <param name="name">Section Name</param>
public class Section(
    RawLine line,
    string name)
        : TrackableBase(line)
{
    /// <summary>
    /// Gets or Sets the Name
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or Sets the FileLineNumberEnd of this Section
    /// </summary>
    public int FileLineNumberEnd { get; set; } = -1;

    /// <summary>
    /// Gets or Sets the Sections list
    /// </summary>
    public List<SubSection> SubSections { get; set; } = [];
}
