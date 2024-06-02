# Aspire.Git

## Installation

Get the package from nuget.

## How to use

Add this configuration to you apphost.

```csharp
builder.AddGitRepository(c => c
	.WithGitUrl()
	.WithName()
	.WithCloneTargetPath()
	.WithDefaultBranch()
	.WithProjectPath())
```

Go into a terminal and navigate to your apphost project and run this command or start it using vs 2022.

```powershell
dotnet run
```