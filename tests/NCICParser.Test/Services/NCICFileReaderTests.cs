using NCICParser.Models;
using NCICParser.Services;

namespace NCICParser.Tests.Services;

public sealed class NCICFileReaderTests
{
    [Fact]
    public void NCICFileReader_ReturnsValidRawLines_WhenFileIsValid()
    {
        NCICFileReader reader = new();

        List<RawLine> fileLines = reader.ReadFile(@".\Services\TestValidFile.txt");

        int maxFileLineNumber = fileLines.Max(i => i.FileLineNumber);
        int maxPageNumber = fileLines.Max(i => i.PageNumber);

        // Valid Code files will have the desired Table of Contents rows.
        List<RawLine> tocLines = [.. fileLines.Where(i => i.IsTableOfContents)];

        Assert.NotEmpty(fileLines);
        Assert.Equal(3478, fileLines.Count);
        Assert.Equal(51, maxPageNumber);
        Assert.Equal(3478, maxFileLineNumber);
        Assert.Equal(3, tocLines.Count);
    }

    [Fact]
    public void NCICFileReader_ReturnsInvalidRawLines_WhenFileIsNotNCICCodeFile()
    {
        NCICFileReader reader = new();

        List<RawLine> fileLines = reader.ReadFile(@".\Services\TestInvalidFile.txt");

        int maxFileLineNumber = fileLines.Max(i => i.FileLineNumber);
        int maxPageNumber = fileLines.Max(i => i.PageNumber);

        // Valid Code files will have the desired Table of Contents rows.
        List<RawLine> tocLines = [.. fileLines.Where(i => i.IsTableOfContents)];

        Assert.NotEmpty(fileLines);
        Assert.Equal(55, fileLines.Count);
        Assert.Equal(1, maxPageNumber); // Should never incriment page count
        Assert.Equal(55, maxFileLineNumber);
        Assert.Empty(tocLines);
    }
}
