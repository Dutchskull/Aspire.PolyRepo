using System.Diagnostics;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> AddGitProject(
        this IDistributedApplicationBuilder builder,
        string gitUrl,
        string? name = null,
        string clonePath = ".",
        string? projectPath = null)
    {
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = "git",
                Arguments = $"clone {gitUrl} {clonePath}",
            }
        };

        process.Start();
        process.WaitForExit();

        string projectName = name ?? GetProjectNameFromGitUrl(gitUrl);
        string resolvedProjectPath = projectPath ?? clonePath;

        if (!Directory.Exists(resolvedProjectPath))
        {
            string message = string.Format("Project folder {0} not found", resolvedProjectPath);
            throw new Exception(message);
        }

        return builder.AddProject(projectName, resolvedProjectPath);
    }

    private static string GetProjectNameFromGitUrl(string gitUrl)
    {
        if (gitUrl.EndsWith(".git"))
        {
            gitUrl = gitUrl[..^4];
        }

        return gitUrl.Split('/')[^1];
    }
}