using System.Text.RegularExpressions;

namespace NCICParser.Models;

/// <summary>
/// Generated Regex Attributes used internally by the RawLine class
/// </summary>
public partial class RawLine
{
    /// <summary>
    /// Matches on known 'Table of Contents' line patterns
    /// </summary>
    [GeneratedRegex(@"^\s*Table of Contents\s*$")]
    private static partial Regex TableOfContentsRegex();

    /// <summary>
    /// Matches on known Section Header line patterns
    /// </summary>
    [GeneratedRegex(@"^\f\s*\d*(\.\d*)?\s-\s.*")]
    private static partial Regex SectionHeaderRegex();

    /// <summary>
    /// Matches on known Category line patterns
    /// </summary>
    [GeneratedRegex(@"^[A-Za-z0-9/]+:\s*[A-Za-z0-9 ]?")]
    private static partial Regex CategoryLineRegex();

    /// <summary>
    /// Matches on known Code Header line patterns
    /// </summary>
    [GeneratedRegex(@"^(?!.*=)(?![ 0-9])(?!\S*/)\S{3,}(?:\s+\S+)*\s{2,}\S+")]
    private static partial Regex CodeHeaderRegex();

    /// <summary>
    /// Matches on known Column Header Values / Specification line patterns
    /// </summary>
    [GeneratedRegex(@"(?<=^|\s{2,})\S.*?(?=\s{2,}|$)")]
    private static partial Regex ColumnSpecsRegex();
}
