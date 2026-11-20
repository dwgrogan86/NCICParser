using System.Text.RegularExpressions;

namespace NCICParser.src;

internal partial class SubSection
{
    /// <summary>
    /// Matches on known Sub Section Header line patterns
    /// </summary>
    [GeneratedRegex(@"(?<=\()[^)]*(?=\))")]
    private static partial Regex SubSectionHeaderRegex();
}
