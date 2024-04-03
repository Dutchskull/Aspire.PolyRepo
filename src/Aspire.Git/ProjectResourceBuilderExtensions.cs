namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<GitRepositoryResource> AddGitRepository(
        this IDistributedApplicationBuilder builder,
        string gitUrl,
        string? name = null,
        string repositoryPath = ".",
        string relativeProjectPath = ".")
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(repositoryPath), gitProjectName);

        string projectName = name ?? gitProjectName;

        string resolvedProjectPath = Path.Join(resolvedRepositoryPath, relativeProjectPath);

        GitRepositoryResource gitRepositoryResource = new(projectName, resolvedRepositoryPath, resolvedProjectPath);

        if (!Directory.Exists(gitRepositoryResource.RepositoryPath))
        {
            ProcessCommands.CloneGitRepository(gitUrl, gitRepositoryResource.RepositoryPath);
        }

        if (!File.Exists(gitRepositoryResource.ProjectPath))
        {
            string message = string.Format("Project folder {0} not found", gitRepositoryResource.ProjectPath);
            throw new Exception(message);
        }

        return builder.CreateResourceBuilder(gitRepositoryResource);
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