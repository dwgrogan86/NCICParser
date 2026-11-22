using NCICParser.Models;
using System.Diagnostics.CodeAnalysis;

namespace NCICParser.Tests.Models;

[ExcludeFromCodeCoverage]
public sealed class SubSectionTests
{
    [Theory]
    [InlineData("Test (ABC)", "ABC")]
    [InlineData("Test (ABC, DEF)", "ABC/DEF")]
    [InlineData("Test (ABC,DEF)", "ABC/DEF")]
    [InlineData("Test", "")]
    public void SubSectionCodeBuildsCodeSetFromName(string name, string expected)
    {
        RawLine source = new("Test", 1, 1, 1);
        SubSection subSection = new(source, name);

        Assert.Equal(expected, subSection.SubSectionCode());
    }

    [Fact]
    public void GetCodeRegexPatternUsesColumnWidths()
    {
        RawLine source = new("Test", 1, 1, 1);
        SubSection subSection = new(source, "Test")
        {
            CodeColumns =
            [
                new CodeColumn ("Code", 0) { ColumnWidth = 4 },
                new CodeColumn ("Value", 4) { ColumnWidth = 8 }
            ]
        };

        Assert.Equal("^(.{1,4})(.{1,})", subSection.GetCodeRegExPattern());
    }
}
