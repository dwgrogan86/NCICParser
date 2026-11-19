using System;
using System.Collections.Generic;
using System.Text;

namespace NCICParser;

/// <summary>
/// Represents a single Section Group
/// </summary>
/// <param name="line">Starting RawLine</param>
/// <param name="name">Group Name</param>
internal class SectionGroup(
    RawLine line,
    string name) 
        : NamedTrackedItem(line, name)
{
    /// <summary>
    /// Gets or Sets the FileLineNumberEnd of this Group
    /// </summary>
    public int FileLineNumberEnd { get; set; } = -1;

    /// <summary>
    /// Gets or Sets the Sections list
    /// </summary>
    public List<Section> Sections { get; set; } = [];
}
