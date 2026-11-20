# NCICParser

NCICParser converts the FBI NCIC Code Manual from a layout-preserved text file into structured, normalized data suitable for database import, analytics, and lookup applications.

The parser was developed specifically to address the NCIC Codes, March 31, 2023 edition with the challenges it presented, including:

- Variable-width table layouts
- Multiple table formats within a single document
- Wrapped records
- Sections spanning multiple pages
- Repeated headers caused by page breaks
- Embedded narrative text
- Category-List relationships

Rather than relying on a fixed schema, the parser dynamically discovers table structures from the document itself and normalizes the resulting data.

---

## Overview

The NCIC Code Manual contains thousands of codes organized into hundreds of sections. While the data appears tabular, the document contains numerous formatting inconsistencies that make extraction difficult.

Examples include:

- Sections repeated across page breaks
- Records wrapped onto multiple lines
- Variable column counts
- Empty trailing columns
- Narrative paragraphs embedded between code lists
- Category-List code hierarchies

NCICParser handles these cases automatically through a combination of document structure discovery and post-processing normalization.

---

## Input

The parser expects a text file generated from the NCIC PDF using Poppler's `pdftotext` utility with the `-layout` option enabled.

```bash
pdftotext -layout nciccodemanual.pdf nciccodemanual.txt
```

The `-layout` option is required because the parser relies on preserved spacing, line breaks, and page boundaries.

---

## Parsing Strategy

### 1. Section Discovery

Sections are identified from the document's table of contents.

### 2. SubSection Discovery

SubSections are discovered dynamically within each section.

### 3. Dynamic Table Discovery

The parser calculates column positions from table headers and builds parsing patterns dynamically.

### 4. Post-Processing Normalization

- Collapse duplicated sections
- Collapse duplicated subsections
- Collapse wrapped records
- Normalize multi-page content

---

## Example Input

```text
Code                                   Article Name                            Description

DCIRCUI                                ACCELERATOR BOARD

EADAPT0                                ADAPTOR                                 ANY DEVICE FOR CONNECTING TWO PARTS, SUCH
                                                                               AS ONES THAT ARE OF DIFFERENT SIZES OR HAVE
                                                                               DIFFERENT MATING FITMENTS
```

## Example Output

```csv
CodeSet,SectionName,ColumnOrdinal,ColumnName,Value,LineNumber
TYP,Article Type (TYP) Field Codes,1,Code,EADAPT0,123
TYP,Article Type (TYP) Field Codes,2,Article Name,ADAPTOR,123
```

## Requirements

- .NET 8+
- Poppler pdftotext
- NCIC Code Manual PDF

## Disclaimer

This project is an independent parser for the NCIC Code Manual and is not affiliated with, endorsed by, or maintained by the FBI or the NCIC program.
