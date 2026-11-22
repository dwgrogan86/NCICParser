# Contributing

Thank you for your interest in NCICParser. Contributions that improve parsing accuracy, test coverage, documentation, or maintainability are welcome.

## Development setup

- Install the .NET 10 SDK.
- Open `NCICParser.slnx` in Visual Studio or build it from the repository root:

  ```shell
  dotnet build NCICParser.slnx
  ```

- Run the test suite:

  ```shell
  dotnet test NCICParser.slnx
  ```

## Making changes

1. Create a focused branch for the change.
2. Add or update tests for behavior changes and edge cases.
3. Preserve the layout-sensitive parsing conventions used by the project.
4. Build and test the solution before submitting a pull request.
5. Keep pull requests small and explain the problem and proposed solution.

## Coding expectations

- Keep changes consistent with the existing C# style.
- Prefer clear, maintainable code over parser-specific cleverness.
- Do not commit generated output, `.vs/`, `bin/`, `obj/`, or `TestResults/` directories.
- Do not commit credentials, local paths, personal data, or environment-specific configuration.
- Do not add large source manuals unless redistribution rights have been confirmed.

## Reporting parsing problems

Include the affected NCIC code-manual edition, a minimal layout-preserved excerpt, the actual output, and the expected output. Remove any sensitive or unrelated text before posting the example.

## Pull requests

By submitting a contribution, you agree that it may be distributed under the repository's MIT License.
