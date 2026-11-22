using NCICParser.Models;
using System.Diagnostics.CodeAnalysis;

namespace NCICParser.Tests.Models;

[ExcludeFromCodeCoverage]
public sealed class RawLineTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(333123)]
    public void RawLineWhenPageNumberChanges_ReturnsNewValue(int value)
    {
        RawLine line = new("", 1, value, 1);

        Assert.Equal(value, line.PageNumber);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(333123)]
    public void RawLineWhenPageLineNumberChanges_ReturnsNewValue(int value)
    {
        RawLine line = new("", 1, 1, value);

        Assert.Equal(value, line.PageLineNumber);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(333123)]
    public void RawLineWhenFileLineNumberChanges_ReturnsNewValue(int value)
    {
        RawLine line = new("", value, 1, 1);

        Assert.Equal(value, line.FileLineNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\f")]
    [InlineData("\t   ")]
    public void RawLineWhenMatchesIsNullOrWhitespace_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.True(line.IsNullOrWhiteSpace);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abc   ")]
    [InlineData("   abc")]
    [InlineData("\f   abc")]
    public void RawLineWhenNotMatchesIsNullOrWhitespace_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.False(line.IsNullOrWhiteSpace);
    }

    [Theory]
    [InlineData("\f")]
    [InlineData("\f   ")]
    [InlineData("\fSome Text")]
    [InlineData("\f Some Text")]
    public void RawLineWhenMatchesFormFeedLine_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.True(line.IsFormFeedLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some Text")]
    [InlineData("Some\fText")]
    public void RawLineWhenNotMatchesFormFeedLine_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.False(line.IsFormFeedLine);
    }

    [Theory]
    [InlineData("XYZ:")]
    [InlineData("XY/Z:")]
    [InlineData("XYZ:ABC")]
    [InlineData("XYZ:ABC/ABC")]
    [InlineData("XYZ:ABC/ ABC")]
    [InlineData("XYZ:ABC / ABC")]
    [InlineData("XYZ:ABC /ABC")]
    [InlineData("XYZ: ABC")]
    [InlineData("XYZ: ABC/ABC")]
    [InlineData("XYZ: ABC/ ABC")]
    [InlineData("XYZ: ABC / ABC")]
    [InlineData("XYZ: ABC /ABC")]
    [InlineData("XY/Z:ABC")]
    [InlineData("XY/Z:ABC/ABC")]
    [InlineData("XY/Z:ABC/ ABC")]
    [InlineData("XY/Z:ABC / ABC")]
    [InlineData("XY/Z:ABC /ABC")]
    [InlineData("XY/Z: ABC")]
    [InlineData("XY/Z: ABC/ABC")]
    [InlineData("XY/Z: ABC/ ABC")]
    [InlineData("XY/Z: ABC / ABC")]
    [InlineData("XY/Z: ABC /ABC")]
    public void RawLineWhenMatchesCategory_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.True(line.IsCategoryLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\f")]
    [InlineData("XYZ")]
    [InlineData("\fXYZ")]
    [InlineData(" XYZ:")]
    [InlineData(" XYZ:Test")]
    [InlineData(" XY/Z:Test")]
    public void RawLineWhenNotMatchesCategory_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.Equal(value, line.Text);
        Assert.False(line.IsCategoryLine);
    }

    [Theory]
    [InlineData("08/15/2026")]
    [InlineData("August 15, 2026")]
    [InlineData("2026-08-15")]
    public void RawLineWhenMatchesDateLine_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.True(line.IsDateLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Not a date")]
    [InlineData("2026/99/99")]
    public void RawLineWhenNotMatchesDateLine_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.False(line.IsDateLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("\f")]
    [InlineData("Table of Contents Page 1")]
    public void RawLineWhenNotMatchesTableOfContents_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.False(line.IsTableOfContents);
    }

    [Theory]
    [InlineData("Table of Contents")]
    [InlineData("  Table of Contents  ")]
    public void RawLineWhenMatchesTableOfContents_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.True(line.IsTableOfContents);
    }



    [Theory]
    [InlineData("\f1 - Test Section")]
    [InlineData("\f 1.2 - Test Section")]
    [InlineData("\f - Test Section")]
    public void RawLineWhenMatchesSectionHeader_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.True(line.IsSectionHeader);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1 - Test Section")]
    [InlineData("\f1-Test Section")]
    [InlineData("\f1.2 Test Section")]
    public void RawLineWhenNotMatchesSectionHeader_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.False(line.IsSectionHeader);
    }


    [Theory]
    [InlineData("Code  Description")]
    [InlineData("CODE  Description Value")]
    public void RawLineWhenMatchesCodeHeader_ReturnsTrue(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.True(line.IsCodeHeader);
    }

    [Theory]
    [InlineData("")]
    [InlineData("=Code  Description")]
    [InlineData(" Code  Description")]
    [InlineData("Code/Set  Description")]
    public void RawLineWhenNotMatchesCodeHeader_ReturnsFalse(string value)
    {
        RawLine line = new(value, 1, 1, 1);

        Assert.False(line.IsCodeHeader);
    }


    [Fact]
    public void RawLineWhenValidCodeHeader_ReturnsColumnSpecs()
    {
        RawLine line = new("Code  Description  Value", 1, 1, 1);

        List<CodeColumn> columns = line.GetColumnSpecs();

        Assert.Equal(3, columns.Count);
        Assert.Equal("Code", columns[0].ColumnName);
        Assert.Equal(0, columns[0].ColumnStart);
        Assert.Equal(6, columns[0].ColumnWidth);

        Assert.Equal("Description", columns[1].ColumnName);
        Assert.Equal(6, columns[1].ColumnStart);
        Assert.Equal(13, columns[1].ColumnWidth);

        Assert.Equal("Value", columns[2].ColumnName);
        Assert.Equal(19, columns[2].ColumnStart);
        Assert.Equal(255, columns[2].ColumnWidth);
    }

    [Fact]
    public void RawLineWhenNotValidCodeHeader_ReturnsEmptyList()
    {
        RawLine line = new("Plain Text", 1, 1, 1);

        List<CodeColumn> columns = line.GetColumnSpecs();

        Assert.Empty(columns);
    }

    [Fact]
    public void RawLineIndexReturnsZeroBasedFileLinePosition()
    {
        RawLine line = new("Text", 10, 1, 1);

        Assert.Equal(9, line.Index);
    }

    [Fact]
    public void RawLineToStringIncludesFileLineAndText()
    {
        RawLine line = new("Test text", 17, 2, 5);

        Assert.Equal("Line: 17: \"Test text\"", line.ToString());
    }
}
