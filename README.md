# Aspire.Git

## Installation

Get the package from nuget.

## How to use

Add this configuration to you apphost.

```csharp
var dotnetGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("dotnetProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.Web/Aspire.Git.Web.csproj"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddProject();

var npmGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("npmProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.React"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddNodeApp();

var nodeGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("nodeProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.Node"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddNpmApp();
```

Go into a terminal and navigate to your apphost project and run this command or start it using vs 2022.

```powershell
dotnet run
```