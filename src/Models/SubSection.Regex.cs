using System.Text.RegularExpressions;

namespace NCICParser.Models;

public partial class SubSection
{
    /// <summary>
    /// Matches on known Sub Section Header line patterns
    /// </summary>
    [GeneratedRegex(@"(?<=\()[^)]*(?=\))")]
    private static partial Regex SubSectionHeaderRegex();
}
