# Aspire.Git

## Installation

Get the package from [nuget](https://www.nuget.org/packages/Dutchskull.Aspire.Git/).

```
dotnet add package Dutchskull.Aspire.Git --version 0.1.0
```

## How to use

Add this configuration to you app host.

```csharp
builder.AddGitRepository(c => c
	.WithGitUrl()
	.WithName()
	.WithCloneTargetPath()
	.WithDefaultBranch()
	.WithProjectPath())
```

Go into a terminal and navigate to your app host project and run this command or start it using vs 2022.

```powershell
dotnet run
```
