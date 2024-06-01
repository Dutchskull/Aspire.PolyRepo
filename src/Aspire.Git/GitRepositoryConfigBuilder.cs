namespace Aspire.Git;

public class GitRepositoryConfigBuilder
{
    private string _branch = "develop";
    private string _gitUrl = string.Empty;
    private string? _name;
    private string _relativeProjectPath = ".";
    private string _repositoryPath = ".";

    public GitRepositoryConfig Build()
    {
        if (string.IsNullOrEmpty(_gitUrl))
        {
            throw new InvalidOperationException("GitUrl must be provided");
        }

        return new GitRepositoryConfig
        {
            GitUrl = _gitUrl,
            Name = _name,
            RepositoryPath = _repositoryPath,
            RelativeProjectPath = _relativeProjectPath,
            Branch = _branch,
        };
    }

    public GitRepositoryConfigBuilder WithDefaultBranch(string branch)
    {
        _branch = branch;
        return this;
    }

    public GitRepositoryConfigBuilder WithGitUrl(string gitUrl)
    {
        _gitUrl = gitUrl;
        return this;
    }

    public GitRepositoryConfigBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public GitRepositoryConfigBuilder WithRelativeProjectPath(string relativeProjectPath)
    {
        _relativeProjectPath = relativeProjectPath;
        return this;
    }

    public GitRepositoryConfigBuilder WithRepositoryPath(string repositoryPath)
    {
        _repositoryPath = repositoryPath;
        return this;
    }
}