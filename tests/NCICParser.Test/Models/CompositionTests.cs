using NCICParser.Models;
using System.Diagnostics.CodeAnalysis;

namespace NCICParser.Tests.Models;

[ExcludeFromCodeCoverage]
public sealed class CompositionTests
{
    [Fact]
    public void CodeItemRowUsesComposedSourceLine()
    {
        RawLine source = new("ABC", 42, 3, 7);

        CodeItemRow row = new(source);

        Assert.Same(source, row.SourceLine);
        Assert.Empty(row.CodeItemValues);
        Assert.False(row.Rejected);
        Assert.Empty(row.CategoryCode);
        Assert.Empty(row.CategoryDescription);
    }

    [Fact]
    public void CategoryPropertiesSplitRawCategory()
    {
        RawLine source = new("ABC", 1, 1, 1);
        CodeItemRow row = new(source)
        {
            CategoryRaw = "XYZ: Test Category"
        };

        Assert.Equal("XYZ", row.CategoryCode);
        Assert.Equal("Test Category", row.CategoryDescription);
    }

    [Fact]
    public void CodeItemsEmptyWhenNoValuesOrOnlyWhitespace()
    {
        RawLine source = new("ABC", 1, 1, 1);
        CodeItemRow row = new(source)
        {
            CodeItemValues = ["", "   "]
        };

        Assert.True(row.CodeItemsEmpty);
    }

    [Fact]
    public void SectionAndSubSectionUseComposedSourceLines()
    {
        RawLine sectionLine = new("\f1 - Test Section", 10, 2, 1);
        RawLine subSectionLine = new("\f1.1 - Test SubSection", 20, 3, 1);

        Section section = new(sectionLine, "Test Section");
        SubSection subSection = new(subSectionLine, "Test SubSection (ABC)");
        section.SubSections.Add(subSection);

        Assert.Same(sectionLine, section.SourceLine);
        Assert.Same(subSectionLine, subSection.SourceLine);
        Assert.Same(subSection, section.SubSections.Single());
    }

    [Fact]
    public void ParseRejectIsCreatedFromSourceLineAndName()
    {
        RawLine source = new("Bad row", 12, 2, 6);

        ParseReject reject = new(source, "Test Section");

        Assert.Same(source, reject.SourceLine);
        Assert.Equal("Test Section", reject.Name);
        Assert.Equal("Column Map No Matched", reject.Reason);
    }

    [Fact]
    public void CodeExportRowAcceptsRequiredInitializedValues()
    {
        RawLine source = new("ABC", 21, 4, 8);

        CodeExportRow row = new(source)
        {
            CodeSet = "A/B",
            SectionName = "Section",
            SubSectionName = "Subsection",
            CategoryCode = "XYZ",
            CategoryDescription = "Category",
            ColumnNumber = 1,
            ColumnName = "Code",
            Value = "ABC"
        };

        Assert.Same(source, row.SourceLine);
        Assert.Equal("A/B", row.CodeSet);
        Assert.Equal(1, row.ColumnNumber);
        Assert.Equal("ABC", row.Value);
    }
}
