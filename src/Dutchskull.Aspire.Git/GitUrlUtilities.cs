namespace Dutchskull.Aspire.Git;

internal static class GitUrlUtilities
{
    internal static string GetProjectNameFromGitUrl(string gitUrl)
    {
        if (gitUrl.EndsWith(".git"))
        {
            gitUrl = gitUrl[..^4];
        }

        return gitUrl.Split('/')[^1];
    }
}