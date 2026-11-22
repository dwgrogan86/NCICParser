# NCICParser Architecture

## Purpose

NCICParser converts layout-preserved text extracted from an NCIC Code Manual PDF into normalized CSV data. It is designed around fixed-width document formatting, form-feed page boundaries, section/subsection discovery, wrapped records, and repeated headers.

## Design Goals
- Avoid hard-coded column definitions.
- Support multiple NCIC manual editions.
- Preserve traceability back to source lines.
- Prefer layout discovery over manual configuration.
- Produce output suitable for relational database import.

## High-Level Flow

```text
NCIC PDF
  -> pdftotext -layout
  -> text file
  -> NCICFileReader
  -> RawLine objects
  -> Parser
  -> Section / SubSection / CodeItemRow models
  -> CsvExporter
  -> NCICCodes.csv
  -> NCICRejects.csv
```

The executable in `src/Program.cs` constructs `NCICFileReader`, `Parser`, and `CsvExporter`, then exports to the application base directory.

## Core Components

### Interfaces

The interfaces under `src/Interfaces/` define the main service boundaries:

- `INCICFileReader` reads the source text and creates `RawLine` objects.
- `IParser` transforms raw lines into a section hierarchy and parsed code rows.
- `ICsvExporter` maps the parsed hierarchy into accepted and rejected CSV records.

### `NCICFileReader`

`NCICFileReader.ReadFile` reads the text file line by line. It calculates:

- File line number
- Page number
- Page line number

It increments the page number when a line begins with a form-feed character. This relies on layout-preserved input produced with `pdftotext -layout`.

### `RawLine`

`RawLine` represents one source line and identifies structural text patterns, including:

- Form feeds
- Table-of-contents entries
- Section headers
- Code headers
- Category lines
- Date lines
- Empty or whitespace-only lines

It also calculates fixed-width column specifications from code-header text.

### `Parser`

The parser receives `RawLine` instances and performs discovery and parsing in stages:

1. Discover sections from table-of-content lines and form-feed boundaries.
2. Discover subsections within each section's line range.
3. Close subsection ranges at section boundaries.
4. Parse each subsection's code list using dynamically calculated column widths.
5. Collapse duplicate sections, duplicate subsections, and wrapped or repeated code rows.

The parser intentionally creates duplicate/overlapping artifacts during discovery because repeated headers and multi-page sections make this the simplest intermediate representation. `CollapseAll` performs the final normalization pass.

### Models

- `Section` represents a top-level manual section and contains subsections.
- `SubSection` represents a named subsection, column definitions, and code rows.
- `CodeColumn` represents a column name and fixed-width boundary.
- `CodeItemRow` represents one logical code row and its values.
- `CodeExportRow` represents a normalized, CSV-ready value record.
- `ParseReject` represents a source line or row rejected during parsing.
- `TrackableBase` supplies shared source-line tracking.

### `CsvExporter`

The exporter flattens the section/subsection hierarchy into two result types:

- `NCICCodes.csv` contains one record per code value and column.
- `NCICRejects.csv` contains source text and reasons for rejected rows.

The exporter quotes values and escapes embedded quotes so output remains CSV-compatible.

## Input Assumptions

The parser assumes that input text preserves:

- Form-feed characters
- Fixed-width column spacing
- Table-of-contents structure
- Section and subsection headers
- Code-header column positions
- Page boundaries

Plain text formats that remove form feeds or collapse spacing may not be compatible.

## Normalization

Normalization is a separate post-processing responsibility:

- Duplicate sections are merged.
- Duplicate subsections are merged.
- Wrapped code-item values are combined with their source rows.
- Repeated page or table headers are removed or ignored.

These rules are implemented in `Parser.CollapseAll` and the associated collapse methods.

## Output Shape

`NCICCodes.csv` uses a normalized, column-oriented format:

```text
CodeSet
SectionName
SubSectionName
CategoryCode
CategoryDescription
Column
Name
Value
PageNumber
PageLineNumber
FileLineNumber
```

This shape allows database imports and pivot operations. `src/Resources/SQL Config.sql` provides a Microsoft SQL Server-specific import/pivot example.

## Error Handling

Parsing and export failures should be surfaced through normal .NET exceptions or recorded in the reject output where the source line itself is not parseable. `NCICRejects.csv` is intended for manual review and does not replace parser logging or unit tests.

## Testing

Tests under `tests/NCICParser.Test/` cover:

- Raw-line classification
- Section/subsection composition
- Dynamic column parsing
- Valid and invalid file reading
- Repeated and continuation record behavior

Add a focused test under the matching model or service test class when changing parsing behavior.
