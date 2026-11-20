using System;
using System.Collections.Generic;
using System.Text;

namespace NCICParser.src;

/// <summary>
/// Represents a high level Section
/// </summary>
/// <param name="line">Starting RawLine</param>
/// <param name="name">Section Name</param>
internal class Section(
    RawLine line,
    string name) 
        : NamedTrackedItem(line, name)
{
    /// <summary>
    /// Gets or Sets the FileLineNumberEnd of this Section
    /// </summary>
    public int FileLineNumberEnd { get; set; } = -1;

    /// <summary>
    /// Gets or Sets the Sections list
    /// </summary>
    public List<SubSection> SubSections { get; set; } = [];
}
