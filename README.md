# Aspire.Git

Aspire.Git is a .NET Aspire package designed to simplify the process of cloning and managing Git repositories within your .NET Aspire applications. This package allows you to configure and use Git repositories seamlessly, integrating them into your cloud-native development workflow.

## Features

- Clone Git repositories directly into your .NET Aspire application.
- Configure repository URL, name, target path, default branch, and project path.
- Easy integration with .NET Aspire App Host.

## Roadmap

This will be updated when more features are thought off.

- Keep git branch updated on each startup
- Using [libgit2sharp](https://github.com/libgit2/libgit2sharp) for the git management

## Installation

To install the Aspire.Git package, use the .NET CLI. Run the following command in your terminal:

```sh
dotnet add package Dutchskull.Aspire.Git
```

## Usage

To use Aspire.Git in your .NET Aspire application, follow these steps:

Add the configuration to your App Host project.

```csharp
builder.AddProjectGitRepository(c => c
    .WithGitUrl("<your-git-url>")
    .WithName("<repository-name>")
    .WithCloneTargetPath("<clone-target-path>")
    .WithDefaultBranch("<default-branch>")
    .WithProjectPath("<project-path>"));

builder.AddNpmGitRepository(c => ...);

builder.AddNodeGitRepository(c => ...);
```

Navigate to your App Host project directory in the terminal.

Run the application using the .NET CLI or Visual Studio 2022.

```sh
dotnet run
```

## Example

Here is an example configuration for adding a Git repository to your .NET Aspire application:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var dotnetGitRepo = builder
    .AddProjectGitRepository(c => c
            .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
            .WithCloneTargetPath("../../repos")
            .WithProjectPath("src/Dutchskull.Aspire.Git.Web/Dutchskull.Aspire.Git.Web.csproj"),
        name: "dotnetProject")
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
```

This configuration clones the specified Git repository into your application, making it available for further development and deployment.

Contributing
Contributions are welcome! If you have any suggestions, bug reports, or feature requests, please open an issue or submit a pull request on GitHub.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
