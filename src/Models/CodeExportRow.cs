namespace NCICParser.Models;

/// <summary>
/// Defines the common output structure to Code csv file.
/// </summary>
/// <param name="line">Current RawLine</param>
public class CodeExportRow(
    RawLine line) 
        : TrackableBase(line)
{
    /// <summary>
    /// Gets or Sets the CodeSet (if determined)
    /// </summary>
    public required string CodeSet { get; init; }

    /// <summary>
    /// Gets or Sets the Section Name
    /// </summary>
    public required string SectionName { get; init; }

    /// <summary>
    /// Gets or Sets the SubSection Name
    /// </summary>
    public required string SubSectionName { get; init; }

    /// <summary>
    /// Gets or Sets the CategoryCode (if any)
    /// </summary>
    public required string CategoryCode { get; init; }

    /// <summary>
    /// Gets or Sets the CategoryDescription (if any)
    /// </summary>
    public required string CategoryDescription { get; init; } 

    /// <summary>
    /// Gets or sets the Ordinal Code ColumnNumber
    /// </summary>
    public required int ColumnNumber { get; init; }

    /// <summary>
    /// Gets or Sets the Code ColumnName
    /// </summary>
    public required string ColumnName { get; init; }

    /// <summary>
    /// Gets or Sets the Code Column Value (if any)
    /// </summary>
    public required string Value { get; init; }
}
