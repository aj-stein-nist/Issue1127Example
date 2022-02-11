# Issue1127Example

## Background

This is a reproduction repo to explain and experiment with [usnistgov/OSCAL#1127](https://github.com/usnistgov/OSCAL/issues/1127) with a minimal viable examples.

1. [good.xsd](./Issue1127Example/good.xsd), an example XML Schema that does not causes the exception in question.
1. [bad.xsd](./Issue1127Example/good.xsd), an example XML Schema that will cause the exception in question.

## Build Environment

This was developed and compiled on a recent, updated system with Windows 10 Version 20H2 (OS Build 19042.1469) and [Microsoft Visual Studio Community 2022 (64-bit) Version 17.0.6](https://visualstudio.microsoft.com/) with the default .NET Runtime set to 4.8.04084, with the compilation profile set to .NET 6.0.

## Using on Local Workstation

To use this, download the repository in your preferred location on your filesystem. Open the `Regexted.sln` solution file for the Visual Studio project, or run the following commands for a configured Visual Studio 2022 Developer PowerShell and/or Visual Studio 2022 Developer Command Prompt.

```sh
dotnet build
dotnet test
```

## Testing Matrix

**NOTE**: If you review the C# Xunit test suite, you will notice the names of the classes, XML Schema XSD file, valid XML input, and invalid XML input for the given schema are named according to their ID and located in the [`./Fixtures` sub-directory](./Fixtures/).

| ID | Regexp  | Valid  | Invalid  | Rationale | Schema Fails to Compile w/ Runtime Error? | Accepts Valid XML? | Rejects Invalid XML? | Notes |
|---|---|---|---|---|---|---|---|---|
| SchemaPattern1 | &amp;#x41;  | A  | B  | matches only "A" (numeric character reference) |  | X | X |  |
| SchemaPattern2 | A  | A  | B  | literal |  | X | X |  |
| SchemaPattern3 | [&amp;#x41;-&amp;#x5a;]  | A  | a  | matches A-Z (range) |  | X | X |  |
| SchemaPattern4 | [\d-[0]]\D | 7A | 0A | character class with exclusion |  | X | X |  |
| SchemaPattern5 | &amp;#x10000; | &amp;#x10000; | &amp;#x10001; | Unicode |  | X | X |  |
| SchemaPattern6 | [&amp;#x10000;-&amp;#xeffff;] | &amp;#x10000; | &amp;#x41; | big range of upper Unicode | X |  |  |   |  |
| SchemaPattern7 | [\c-[&amp;#x10000;-&amp;#xeffff;]] | &amp;#x41; | &amp;#x10000; | XML name characters except for big range of upper Unicode | X |  |  |   | |
| SchemaPattern8 | [&amp;#x10000;-&amp;#x10010;] | &amp;#x41; | &amp;#x10000; |  |   |  |  |   | |
