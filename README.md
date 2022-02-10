# Issue1127Example

## Background

This is a reproduction repo to explain and experiment with [usnistgov/OSCAL#1127](https://github.com/usnistgov/OSCAL/issues/1127) with a minimal viable examples.

1. [good.xsd](./Issue1127Example/good.xsd), an example XML Schema that does not causes the exception in question.
1. [bad.xsd](./Issue1127Example/good.xsd), an example XML Schema that will cause the exception in question.

## Build Environment

This was developed and compiled on a recent, updated system with Windows 10 Version 20H2 (OS Build 19042.1469) and [Microsoft Visual Studio Community 2022 (64-bit) Version 17.0.6](https://visualstudio.microsoft.com/) with the default .NET Runtime set to 4.8.04084, with the compilation profile set to .NET 6.0.