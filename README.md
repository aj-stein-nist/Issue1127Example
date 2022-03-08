# Issue1127Example

## Background

This is a reproduction repo to explain and experiment with [usnistgov/OSCAL#1127](https://github.com/usnistgov/OSCAL/issues/1127) with a set of minimal viable examples.

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
| SchemaPattern1 | `&amp;#x41;`  | `A`  | `B`  | matches only "A" (numeric character reference) |  | X | X |  |
| SchemaPattern2 | `A`  | `A`  | `B`  | literal |  | X | X |  |
| SchemaPattern3 | `[&amp;#x41;-&amp;#x5a;]`  | `A`  | `a`  | matches A-Z (range) |  | X | X |  |
| SchemaPattern4 | `[\d-[0]]\D` | `7A` | `0A` | character class with exclusion |  | X | X |  |
| SchemaPattern5 | `&amp;#x10000;` | `&amp;#x10000;` | `&amp;#x10001;` | Unicode |  | X | X |  |
| SchemaPattern6 | `[&amp;#x10000;-&amp;#xeffff;]` | `&amp;#x10000;` | `&amp;#x41;` | big range of upper Unicode | X |  |  | `"The Pattern constraining facet is invalid - Invalid pattern ..."` |
| SchemaPattern7 | `[\c-[&amp;#x10000;-&amp;#xeffff;]]` | `&amp;#x41;` | `&amp;#x10000;` | XML name characters except for big range of upper Unicode | X |  |  |  `"The Pattern constraining facet is invalid - Invalid pattern ..."` |
| SchemaPattern8 | `[&amp;#x10000;-&amp;#x10010;]` | `&amp;#x10001;` | `&amp;#x41;` | More unicode range testing | X |   |  | `"The Pattern constraining facet is invalid - Invalid pattern ..."` |
| SchemaPattern9 | `\c` | `&amp;#x41;` | `%` | More unicode range testing | | X | X |  |
| SchemaPattern10 | `[\c-[&amp;#x10000;-&amp;#xeffff;]]` | `&#x10000;` | `&#x42;` | More unicode range testing | X |   |  | `"The Pattern constraining facet is invalid - Invalid pattern ..."` |
| SchemaPattern11 | `[&amp;#x10000;-&amp;#x10010;]` | `A1` | `1A` | More unicode range testing | X |   |  | `"The Pattern constraining facet is invalid - Invalid pattern ..."` |
| SchemaPattern12 | `[\i-[:]][\c-[:]]*` | `&#x41;1` | `1&#x61;` | More unicode range testing |  | X | X | |
| SchemaPattern13 | `^(\p{L}|_)(\p{L}|\p{N}|[.\-_])*$` | `ğ0.`| `.🚀🛑` | Testing recommended fix from usnistgov/metaschema#191 |  | X | X | |

## What Do Class Properties Mean in Tests?

OK, let's look at this test.

```
        public void Invalid()
        {
            string schema = $"{fixture.contextPath}{pattern}.xsd";
            XDocument document = XDocument.Load($"{fixture.contextPath}{pattern}Invalid.xml");
            XmlValidator validator = new XmlValidator(fixture.namespaceUri, fixture.namespacePrefix, schema, document);
            validator.Validate();
            Assert.True(
                !validator.hasRuntimeErrors &&
                validator.hasValidationErrors &&
                validator.hasCompletedValidation
                );
        }
```

What do the `validator` boolean attributes mean and how do they relate to the testing rubric?

- `validator.hasRuntimeErrors`: when loading the XML document, the schema, or compiling the schema into native form in the .NET runtime, there was an exception. Given our tests, this most always means the XML schema's XSD file is invalid, and what are often testing here. This attribute equates to `Schema Fails to Compile w/ Runtime Error?` in the table above.

- `validator.hasValidationErrors`: when validating the XML document against the schema, `false` means it was validated successfully without errors. A value of `true` means it validated and errors were found. Not used explicitly in the test, the `List<string>` list of validation errors can be retrieved with `validator.validationErrors` attribute of the class instance. This attribute determines whether `Accepts Valid XML?` `Rejects Invalid XML?` columns and explain "did the schema validate as intended?" scenarios.

- `validator.hasCompletedValidation`: when validating the XML document, a schema validation attempt _may_ throw an exception and we want to be aware of that. This field should always have a value of `true` or we had unexpected behavior in our tests, this can be checked in conjuction with `hasRuntimeErrors` to know if schema compilation was successful, validation occurred, but values in a test document caused trouble.
