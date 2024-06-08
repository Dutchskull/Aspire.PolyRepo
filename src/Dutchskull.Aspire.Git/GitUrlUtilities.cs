using System.Text.RegularExpressions;

namespace Dutchskull.Aspire.Git;

internal static partial class GitUrlUtilities
{
    internal static string GetProjectNameFromGitUrl(string gitUrl) => 
        gitUrl.RemovePostfix(".git").Split('/')[^1];

    internal static bool IsValidGitUrl(string url) =>
        !string.IsNullOrEmpty(url) && GitUrlRegex().IsMatch(url);

    [GeneratedRegex(@"^(?:git|https?|git@[\w\.]+):\/\/[\w\.@\:\/\-~]+\.git(?:\/)?$")]
    private static partial Regex GitUrlRegex();
}
