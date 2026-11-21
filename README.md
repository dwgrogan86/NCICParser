# NCICParser

NCICParser converts the FBI NCIC Code Manual from a layout-preserved text file into structured, normalized data suitable for database import, analytics, and lookup applications.

The parser was developed specifically to address the NCIC Codes, March 31, 2023, and June 30, 2026 editions with the challenges they presented, including:

- Variable-width table layouts
- Multiple table formats within a single document
- Wrapped records
- Sections spanning multiple pages
- Repeated headers caused by page breaks
- Embedded narrative text
- Category-List relationships

Rather than relying on a fixed schema, the parser dynamically discovers table structures from the document itself and normalizes the resulting data using the same logic for both source files.

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
This is necessary to preserve the pdf's structure better in the resulting text file than the downloadable text files from such sources as:
https://archive.org/details/NCICCodeManual2026/NCIC-Code-Manual-June-2026/

which remove form-feed page boundaries and collapse column spacing to a single space, making it difficult to distinguish code-list headers from ordinary prose.

The parser must identify column boundaries, associate continuation lines with the correct field, and reconstruct each logical record before normalization.

```bash
# Chose your version - noting that all SQL Config settings expect 2026
pdftotext -layout nciccodemanual.pdf nciccodemanual.txt
pdftotext -layout NCIC-Code-Manual-June-2026.pdf NCIC-Code-Manual-June-2026.txt
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
AER0: AIRCRAFT SEE THE VEHICLE FILE CHAPTER OF THE NCIC OPERATING
MANUAL
Model (VM0)        Description                           Comment
BLM                BLIMP

DRN                DRONE%                                OVER.55 POUNDS AND ARE SUBJECT TO FAA
                                                         REGISTRATION RULES. SMALLER DRONES ARE TO BE
                                                         ENTERED IN THE ARTICLE FILE

HEL                HELICOPTERS AND OTHER ROTOR WING OR CONSIDER AN AUTOGIRO / AUTOGYRO TO BE A
                   ROTARY-BLADE DRIVEN AIRCRAFT%       HELICOPTER FOR CODING PURPOSES
```

## Example Output

```csv
CodeSet,SectionName,SubSectionName,CategoryCode,CategoryDescription,Column,Name,Value,PageNumber,PageLineNumber,FileLineNumber
"VMA/VMO","Vehicle Data Codes","2.2 - Vehicle Make/Brand (VMA) and Model (VMO) for Automobiles, Light-Duty Vans, Light-Duty Trucks, and Parts","AER0","AIRCRAFT SEE THE VEHICLE FILE CHAPTER OF THE NCIC OPERATING MANUAL",1,"Model (VM0)","BLM",780,27,54253
"VMA/VMO","Vehicle Data Codes","2.2 - Vehicle Make/Brand (VMA) and Model (VMO) for Automobiles, Light-Duty Vans, Light-Duty Trucks, and Parts","AER0","AIRCRAFT SEE THE VEHICLE FILE CHAPTER OF THE NCIC OPERATING MANUAL",2,"Description","BLIMP",780,27,54253
"VMA/VMO","Vehicle Data Codes","2.2 - Vehicle Make/Brand (VMA) and Model (VMO) for Automobiles, Light-Duty Vans, Light-Duty Trucks, and Parts","AER0","AIRCRAFT SEE THE VEHICLE FILE CHAPTER OF THE NCIC OPERATING MANUAL",3,"Comment","",780,27,54253
```

### This structure when uploaded to a DB is easily pivot-ready - SEE SQL Config.sql

## Requirements

- .NET 8+
- Poppler pdftotext
- NCIC Code Manual PDF

## Disclaimer

This project is an independent parser for the NCIC Code Manual and is not affiliated with, endorsed by, or maintained by the FBI or the NCIC program.
